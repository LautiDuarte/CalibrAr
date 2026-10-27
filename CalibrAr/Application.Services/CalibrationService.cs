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
        private readonly IReferenceStandardRepository referenceStandardRepository;
        private readonly INonConformityRepository nonConformityRepository;
        private readonly IInstrumentService instrumentService;
        private readonly ISearchRepository searchRepository;


        public CalibrationService(
            ICalibrationRepository calibrationRepository,
            IInstrumentRepository instrumentRepository,
            IInstrumentTypeRepository instrumentTypeRepository,
            IProcedureRepository procedureRepository,
            IUserRepository userRepository,
            IReferenceStandardRepository referenceStandardRepository,
            INonConformityRepository nonConformityRepository,
            IInstrumentService instrumentService,
            ISearchRepository searchRepository)
        {
            this.searchRepository = searchRepository;
            this.nonConformityRepository = nonConformityRepository;
            this.calibrationRepository = calibrationRepository;
            this.instrumentRepository = instrumentRepository;
            this.instrumentTypeRepository = instrumentTypeRepository;
            this.procedureRepository = procedureRepository;
            this.userRepository = userRepository;
            this.referenceStandardRepository = referenceStandardRepository;
            this.instrumentService = instrumentService;
        }

        public async Task<CalibrationDTO> AddAsync(CalibrationDTO dto, int userId)
        {
            var instrument = await GetInstrumentOrThrowAsync(dto.InstrumentId);
            await EnsureProcedureExistsAsync(dto.ProcedureId);
            await EnsureUserExistsAsync(userId);

            var now = DateTime.Now;
            var interventionType = ParseEnum<InterventionType>(dto.InterventionType, nameof(dto.InterventionType));
            var result = ParseEnum<Result>(dto.Result, nameof(dto.Result));

            // Quién la realizó lo define el token, y una calibración nueva siempre nace sin aprobar
            Calibration calibration = new Calibration(0, now, interventionType, dto.IsExternal, dto.ExternalLab, dto.CertificateNumber, result, dto.RestrictionDetail, dto.Notes, now, dto.InstrumentId, dto.ProcedureId, userId, null);

            var maxAllowedError = await ResolveMaxAllowedErrorAsync(instrument);
            calibration.SetMeasurements(BuildMeasurements(dto.Measurements, 0, maxAllowedError, resetIds: true));
            calibration.SetReferenceStandards(await GetReferenceStandardsAsync(dto, now));

            await calibrationRepository.AddAsync(calibration);
            await RefreshInstrumentCalibrationDatesAsync(dto.InstrumentId);

            dto.Id = calibration.Id;
            dto.CalibrationDate = calibration.CalibrationDate;
            dto.CreatedAt = calibration.CreatedAt;
            dto.PerformedByUserId = calibration.PerformedByUserId;
            dto.ApprovedByUserId = null;
            dto.Measurements = calibration.Measurements.Select(MapMeasurementToDto).ToList();
            dto.ReferenceStandardIds = calibration.ReferenceStandards.Select(r => r.Id).ToList();
            FillNavigationNames(dto, calibration);

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await calibrationRepository.GetAsync(id);
            if (existing == null)
                return false;

            var nonConformities = await nonConformityRepository.GetAllAsync();
            if (nonConformities.Any(n => n.CalibrationId == id))
                throw new InvalidOperationException("No se puede eliminar la calibración porque tiene una no conformidad asociada. Elimine primero la no conformidad.");

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

        // Los filtros se resuelven en la base (SearchRepository, ADO.NET); acá solo se cargan y mapean los resultados
        public async Task<IEnumerable<CalibrationDTO>> SearchAsync(DateTime? from, DateTime? to, string? result, int? instrumentId,
            string? text, string? interventionType, bool? approved)
        {
            if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
                throw new ArgumentException("La fecha desde no puede ser posterior a la fecha hasta.");

            Result? parsedResult = string.IsNullOrWhiteSpace(result) ? null : ParseEnum<Result>(result, nameof(result));
            InterventionType? parsedIntervention = string.IsNullOrWhiteSpace(interventionType)
                ? null
                : ParseEnum<InterventionType>(interventionType, nameof(interventionType));

            var ids = await searchRepository.SearchCalibrationIdsAsync(text, from, to, parsedResult, parsedIntervention, approved, instrumentId);
            if (ids.Count == 0)
                return Enumerable.Empty<CalibrationDTO>();

            var calibrations = await calibrationRepository.GetByIdsAsync(ids);
            return calibrations.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(CalibrationDTO dto, int userId)
        {
            var existing = await calibrationRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            var previousInstrumentId = existing.InstrumentId; // capturar antes de guardar

            var instrument = await GetInstrumentOrThrowAsync(dto.InstrumentId);
            await EnsureProcedureExistsAsync(dto.ProcedureId);

            // La aprobación no se define desde acá (para eso está ApproveAsync) y el realizador no se cambia.
            // Lo único que admite esta edición es quitar la aprobación mandándola en null.
            int? approvedByUserId = null;
            if (existing.ApprovedByUserId.HasValue)
            {
                if (dto.ApprovedByUserId.HasValue)
                    throw new InvalidOperationException("La calibración está aprobada: hay que quitarle la aprobación antes de modificarla.");

                await EnsureCanApproveAsync(userId);
            }

            if (dto.CalibrationDate.Date > DateTime.Today)
                throw new ArgumentException("The calibration date cannot be in the future.", nameof(dto.CalibrationDate));

            var interventionType = ParseEnum<InterventionType>(dto.InterventionType, nameof(dto.InterventionType));
            var result = ParseEnum<Result>(dto.Result, nameof(dto.Result));

            Calibration calibration = new Calibration(dto.Id, dto.CalibrationDate, interventionType, dto.IsExternal, dto.ExternalLab, dto.CertificateNumber, result, dto.RestrictionDetail, dto.Notes, existing.CreatedAt, dto.InstrumentId, dto.ProcedureId, existing.PerformedByUserId, approvedByUserId);

            var maxAllowedError = await ResolveMaxAllowedErrorAsync(instrument);
            calibration.SetMeasurements(BuildMeasurements(dto.Measurements, dto.Id, maxAllowedError, resetIds: false));
            calibration.SetReferenceStandards(await GetReferenceStandardsAsync(dto, dto.CalibrationDate));

            var updated = await calibrationRepository.UpdateAsync(calibration);
            if (!updated)
                return false;

            await RefreshInstrumentCalibrationDatesAsync(dto.InstrumentId);
            if (previousInstrumentId != dto.InstrumentId)
                await RefreshInstrumentCalibrationDatesAsync(previousInstrumentId);

            return true;
        }

        public async Task<bool> ApproveAsync(int id, int approverUserId)
        {
            var existing = await calibrationRepository.GetAsync(id);
            if (existing == null)
                return false;

            if (existing.ApprovedByUserId.HasValue)
                throw new InvalidOperationException("The calibration is already approved.");

            await ValidateApproverAsync(approverUserId, existing.PerformedByUserId);

            var calibration = new Calibration(existing.Id, existing.CalibrationDate, existing.InterventionType, existing.IsExternal,
                existing.ExternalLab, existing.CertificateNumber, existing.Result, existing.RestrictionDetail, existing.Notes,
                existing.CreatedAt, existing.InstrumentId, existing.ProcedureId, existing.PerformedByUserId, approverUserId);

            // Las mediciones se conservan como están guardadas: aprobar no recalcula tolerancias
            calibration.SetMeasurements(existing.Measurements.Select(m =>
                new CalibrationMeasurement(m.Id, m.NominalValue, m.MeasuredValue, m.Error, m.IsWithinTolerance, m.Notes, existing.Id)));

            calibration.SetReferenceStandards(existing.ReferenceStandards);

            return await calibrationRepository.UpdateAsync(calibration);
        }

        // Quitar la aprobación lo puede hacer quien podría aprobar: Administrador o Responsable
        private async Task EnsureCanApproveAsync(int userId)
        {
            var user = await userRepository.GetAsync(userId)
                ?? throw new KeyNotFoundException($"There is no user with id {userId}.");

            if (user.Role != UserRole.Administrador && user.Role != UserRole.Responsable)
                throw new ArgumentException("The user does not have permission to approve calibrations.");
        }

        // Reglas para aprobar: el aprobador debe ser Administrador o Responsable,
        // y solo un Administrador puede aprobar una calibración que él mismo realizó
        private async Task ValidateApproverAsync(int approverUserId, int? performedByUserId)
        {
            var approver = await userRepository.GetAsync(approverUserId);
            if (approver == null)
                throw new KeyNotFoundException($"There is no user with id {approverUserId}.");

            if (approver.Role != UserRole.Administrador && approver.Role != UserRole.Responsable)
                throw new ArgumentException("The user does not have permission to approve calibrations.");

            if (approver.Id == performedByUserId && approver.Role != UserRole.Administrador)
                throw new ArgumentException("Only an administrator can approve a calibration that they performed.");
        }

        // La última calibración real (fecha máxima) manda; InstrumentService aplica la regla de fechas
        private async Task RefreshInstrumentCalibrationDatesAsync(int instrumentId)
        {
            var latest = await calibrationRepository.GetLatestByInstrumentAsync(instrumentId);
            if (latest == null)
            {
                await instrumentService.RecalculateCalibrationScheduleAsync(instrumentId, null);
            }
            else
            {
                await instrumentService.RecalculateCalibrationScheduleAsync(instrumentId, latest.CalibrationDate);
            }
        }

        // Los patrones aplican solo a calibraciones internas (trazabilidad metrológica)
        // El certificado del patrón tiene que estar vigente el día de la calibración (no hoy), así una calibración
        // vieja hecha con un patrón que en su momento valía se puede seguir editando.
        private async Task<List<ReferenceStandard>> GetReferenceStandardsAsync(CalibrationDTO dto, DateTime calibrationDate)
        {
            var result = new List<ReferenceStandard>();
            if (dto.IsExternal)
                return result;

            foreach (var id in dto.ReferenceStandardIds.Distinct())
            {
                var standard = await referenceStandardRepository.GetAsync(id)
                    ?? throw new KeyNotFoundException($"There is no reference standard with id {id}.");

                if (standard.CertificateExpiresAt.Date < calibrationDate.Date)
                    throw new ArgumentException($"El certificado del patrón '{standard.Description}' venció el {standard.CertificateExpiresAt:dd/MM/yyyy}: no se puede usar en la calibración.");

                result.Add(standard);
            }
            return result;
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
                Measurements = calibration.Measurements?.Select(MapMeasurementToDto).ToList() ?? new List<CalibrationMeasurementDTO>(),
                ReferenceStandardIds = calibration.ReferenceStandards?.Select(r => r.Id).ToList() ?? new List<int>(),
            };
            FillNavigationNames(dto, calibration);
            return dto;
        }
    }
}