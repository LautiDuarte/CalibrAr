using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using DTOs;
using Domain.Model;

namespace Application.Services
{
    public class CalibrationService : ICalibrationService
    {
        private readonly ICalibrationRepository calibrationRepository;
        private readonly IInstrumentRepository instrumentRepository;
        private readonly IInstrumentTypeRepository instrumentTypeRepository;
        private readonly IProcedureRepository procedureRepository;
        private readonly IUserRepository userRepository;
        private readonly IInstrumentService instrumentService;

        public CalibrationService(
            ICalibrationRepository calibrationRepository,
            IInstrumentRepository instrumentRepository,
            IInstrumentTypeRepository instrumentTypeRepository,
            IProcedureRepository procedureRepository,
            IUserRepository userRepository,
            IInstrumentService instrumentService)
        {
            this.calibrationRepository = calibrationRepository;
            this.instrumentRepository = instrumentRepository;
            this.instrumentTypeRepository = instrumentTypeRepository;
            this.procedureRepository = procedureRepository;
            this.userRepository = userRepository;
            this.instrumentService = instrumentService;
        }

        public async Task<CalibrationDTO> AddAsync(CalibrationDTO dto)
        {
            var instrument = await GetInstrumentOrThrowAsync(dto.InstrumentId);
            await EnsureProcedureExistsAsync(dto.ProcedureId);
            await EnsureUserExistsAsync(dto.PerformedByUserId);
            await EnsureUserExistsAsync(dto.ApprovedByUserId);

            var now = DateTime.Now;
            var interventionType = ParseEnum<InterventionType>(dto.InterventionType, nameof(dto.InterventionType));
            var result = ParseEnum<Result>(dto.Result, nameof(dto.Result));

            Calibration calibration = new Calibration(0, now, interventionType, dto.IsExternal, dto.ExternalLab, dto.CertificateNumber, result, dto.RestrictionDetail, dto.Notes, now, dto.InstrumentId, dto.ProcedureId, dto.PerformedByUserId, dto.ApprovedByUserId);

            var maxAllowedError = await ResolveMaxAllowedErrorAsync(instrument);
            calibration.SetMeasurements(BuildMeasurements(dto.Measurements, 0, maxAllowedError, resetIds: true));

            await calibrationRepository.AddAsync(calibration);
            await RefreshInstrumentCalibrationDatesAsync(dto.InstrumentId);

            dto.Id = calibration.Id;
            dto.CalibrationDate = calibration.CalibrationDate;
            dto.CreatedAt = calibration.CreatedAt;
            dto.Measurements = calibration.Measurements.Select(MapMeasurementToDto).ToList();
            FillNavigationNames(dto, calibration);

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await calibrationRepository.GetAsync(id);
            if (existing == null)
                return false;

            var deleted = await calibrationRepository.DeleteAsync(id);
            if (deleted)
                await RefreshInstrumentCalibrationDatesAsync(existing.InstrumentId);

            return deleted;
        }

        public async Task<CalibrationDTO?> GetAsync(int id)
        {
            Calibration? calibration = await calibrationRepository.GetAsync(id);

            if (calibration == null)
                return null;

            return MapToDto(calibration);
        }

        public async Task<IEnumerable<CalibrationDTO>> GetAllAsync()
        {
            var calibrations = await calibrationRepository.GetAllAsync();
            return calibrations.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(CalibrationDTO dto)
        {
            var existing = await calibrationRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            var previousInstrumentId = existing.InstrumentId; // capturar antes de guardar

            var instrument = await GetInstrumentOrThrowAsync(dto.InstrumentId);
            await EnsureProcedureExistsAsync(dto.ProcedureId);
            await EnsureUserExistsAsync(dto.PerformedByUserId);
            await EnsureUserExistsAsync(dto.ApprovedByUserId);

            if (dto.CalibrationDate.Date > DateTime.Today)
                throw new ArgumentException("The calibration date cannot be in the future.", nameof(dto.CalibrationDate));

            var interventionType = ParseEnum<InterventionType>(dto.InterventionType, nameof(dto.InterventionType));
            var result = ParseEnum<Result>(dto.Result, nameof(dto.Result));

            Calibration calibration = new Calibration(dto.Id, dto.CalibrationDate, interventionType, dto.IsExternal, dto.ExternalLab, dto.CertificateNumber, result, dto.RestrictionDetail, dto.Notes, existing.CreatedAt, dto.InstrumentId, dto.ProcedureId, dto.PerformedByUserId, dto.ApprovedByUserId);

            var maxAllowedError = await ResolveMaxAllowedErrorAsync(instrument);
            calibration.SetMeasurements(BuildMeasurements(dto.Measurements, dto.Id, maxAllowedError, resetIds: false));

            var updated = await calibrationRepository.UpdateAsync(calibration);
            if (!updated)
                return false;

            await RefreshInstrumentCalibrationDatesAsync(dto.InstrumentId);
            if (previousInstrumentId != dto.InstrumentId)
                await RefreshInstrumentCalibrationDatesAsync(previousInstrumentId);

            return true;
        }

        // La última calibración real (fecha máxima) manda; InstrumentService aplica la regla de fechas
        private async Task RefreshInstrumentCalibrationDatesAsync(int instrumentId)
        {
            var latest = await calibrationRepository.GetLatestByInstrumentAsync(instrumentId);
            if (latest == null)
                return; // sin calibraciones: no pisar lo que ya tenga el instrumento

            await instrumentService.RecalculateCalibrationScheduleAsync(instrumentId, latest.CalibrationDate);
        }

        private async Task<Instrument> GetInstrumentOrThrowAsync(int instrumentId)
        {
            var instrument = await instrumentRepository.GetAsync(instrumentId);
            if (instrument == null)
                throw new KeyNotFoundException($"There is no instrument with id {instrumentId}.");
            return instrument;
        }

        // El error del instrumento sobreescribe el del tipo si está definido
        private async Task<decimal?> ResolveMaxAllowedErrorAsync(Instrument instrument)
        {
            if (instrument.MaxAllowedError.HasValue)
                return instrument.MaxAllowedError;

            var type = await instrumentTypeRepository.GetAsync(instrument.InstrumentTypeId);
            return type?.MaxAllowedError;
        }

        private static List<CalibrationMeasurement> BuildMeasurements(IEnumerable<CalibrationMeasurementDTO>? dtos, int calibrationId, decimal? maxAllowedError, bool resetIds)
        {
            var list = dtos?.ToList() ?? new List<CalibrationMeasurementDTO>();

            if (list.Count > 0 && !maxAllowedError.HasValue)
                throw new ArgumentException("Neither the instrument nor its type define a maximum allowed error, so the measurements cannot be evaluated.", nameof(dtos));

            return list.Select(m =>
            {
                var error = m.MeasuredValue - m.NominalValue;
                var isWithinTolerance = Math.Abs(error) <= maxAllowedError!.Value;
                return new CalibrationMeasurement(resetIds ? 0 : m.Id, m.NominalValue, m.MeasuredValue, error, isWithinTolerance, m.Notes, calibrationId);
            }).ToList();
        }

        private async Task EnsureProcedureExistsAsync(int? procedureId)
        {
            if (procedureId == null)
                return;
            var procedure = await procedureRepository.GetAsync(procedureId.Value);
            if (procedure == null)
                throw new KeyNotFoundException($"There is no procedure with id {procedureId}.");
        }

        private async Task EnsureUserExistsAsync(int? userId)
        {
            if (userId == null)
                return;
            var user = await userRepository.GetAsync(userId.Value);
            if (user == null)
                throw new KeyNotFoundException($"There is no user with id {userId}.");
        }

        private static TEnum ParseEnum<TEnum>(string value, string paramName) where TEnum : struct
        {
            if (!Enum.TryParse<TEnum>(value, out var parsed))
                throw new ArgumentException($"The value '{value}' is not valid", paramName);
            return parsed;
        }

        private static void FillNavigationNames(CalibrationDTO dto, Calibration calibration)
        {
            dto.InstrumentCode = calibration.Instrument?.Code;
            dto.ProcedureName = calibration.Procedure?.Name;
            dto.PerformedByUserName = calibration.PerformedByUser?.FullName;
            dto.ApprovedByUserName = calibration.ApprovedByUser?.FullName;
        }

        private static CalibrationMeasurementDTO MapMeasurementToDto(CalibrationMeasurement m)
        {
            return new CalibrationMeasurementDTO
            {
                Id = m.Id,
                NominalValue = m.NominalValue,
                MeasuredValue = m.MeasuredValue,
                Error = m.Error,
                IsWithinTolerance = m.IsWithinTolerance,
                Notes = m.Notes,
                CalibrationId = m.CalibrationId
            };
        }

        private static CalibrationDTO MapToDto(Calibration calibration)
        {
            var dto = new CalibrationDTO
            {
                Id = calibration.Id,
                CalibrationDate = calibration.CalibrationDate,
                InterventionType = calibration.InterventionType.ToString(),
                IsExternal = calibration.IsExternal,
                ExternalLab = calibration.ExternalLab,
                CertificateNumber = calibration.CertificateNumber,
                Result = calibration.Result.ToString(),
                RestrictionDetail = calibration.RestrictionDetail,
                Notes = calibration.Notes,
                CreatedAt = calibration.CreatedAt,
                InstrumentId = calibration.InstrumentId,
                ProcedureId = calibration.ProcedureId,
                PerformedByUserId = calibration.PerformedByUserId,
                ApprovedByUserId = calibration.ApprovedByUserId,
                Measurements = calibration.Measurements?.Select(MapMeasurementToDto).ToList()
                               ?? new List<CalibrationMeasurementDTO>()
            };
            FillNavigationNames(dto, calibration);
            return dto;
        }
    }
}