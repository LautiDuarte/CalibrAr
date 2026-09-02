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
        private readonly IProcedureRepository procedureRepository;
        private readonly IUserRepository userRepository;

        public CalibrationService(ICalibrationRepository calibrationRepository, IInstrumentRepository instrumentRepository, IProcedureRepository procedureRepository, IUserRepository userRepository)
        {
            this.calibrationRepository = calibrationRepository;
            this.instrumentRepository = instrumentRepository;
            this.procedureRepository = procedureRepository;
            this.userRepository = userRepository;
        }

        public async Task<CalibrationDTO> AddAsync(CalibrationDTO dto)
        {
            await EnsureInstrumentExistsAsync(dto.InstrumentId);
            await EnsureProcedureExistsAsync(dto.ProcedureId);
            await EnsureUserExistsAsync(dto.PerformedByUserId);
            await EnsureUserExistsAsync(dto.ApprovedByUserId);

            var createdAt = DateTime.Now;
            var interventionType = ParseEnum<InterventionType>(dto.InterventionType, nameof(dto.InterventionType));
            var result = ParseEnum<Result>(dto.Result, nameof(dto.Result));
            Calibration calibration = new Calibration(0, dto.CalibrationDate, interventionType, dto.IsExternal, dto.ExternalLab, dto.CertificateNumber, result, dto.RestrictionDetail, dto.NextCalibrationDate, dto.Notes, createdAt, dto.InstrumentId, dto.ProcedureId, dto.PerformedByUserId, dto.ApprovedByUserId);

            await calibrationRepository.AddAsync(calibration);

            dto.Id = calibration.Id;
            dto.CreatedAt = calibration.CreatedAt;
            FillNavigationNames(dto, calibration);

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await calibrationRepository.DeleteAsync(id);
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

            await EnsureInstrumentExistsAsync(dto.InstrumentId);
            await EnsureProcedureExistsAsync(dto.ProcedureId);
            await EnsureUserExistsAsync(dto.PerformedByUserId);
            await EnsureUserExistsAsync(dto.ApprovedByUserId);

            var interventionType = ParseEnum<InterventionType>(dto.InterventionType, nameof(dto.InterventionType));
            var result = ParseEnum<Result>(dto.Result, nameof(dto.Result));
            Calibration calibration = new Calibration(dto.Id, dto.CalibrationDate, interventionType, dto.IsExternal, dto.ExternalLab, dto.CertificateNumber, result, dto.RestrictionDetail, dto.NextCalibrationDate, dto.Notes, existing.CreatedAt, dto.InstrumentId, dto.ProcedureId, dto.PerformedByUserId, dto.ApprovedByUserId);
            return await calibrationRepository.UpdateAsync(calibration);
        }

        private async Task EnsureInstrumentExistsAsync(int instrumentId)
        {
            var instrument = await instrumentRepository.GetAsync(instrumentId);
            if (instrument == null)
                throw new KeyNotFoundException($"There is no instrument with id {instrumentId}.");
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
                NextCalibrationDate = calibration.NextCalibrationDate,
                Notes = calibration.Notes,
                CreatedAt = calibration.CreatedAt,
                InstrumentId = calibration.InstrumentId,
                ProcedureId = calibration.ProcedureId,
                PerformedByUserId = calibration.PerformedByUserId,
                ApprovedByUserId = calibration.ApprovedByUserId
            };
            FillNavigationNames(dto, calibration);
            return dto;
        }
    }
}