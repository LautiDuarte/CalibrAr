using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data;
using DTOs;
using Domain.Model;

namespace Application.Services
{
    public class NonConformityService : INonConformityService
    {
        private readonly INonConformityRepository nonConformityRepository;
        private readonly IInstrumentRepository instrumentRepository;
        private readonly ICalibrationRepository calibrationRepository;
        private readonly IUserRepository userRepository;

        public NonConformityService(INonConformityRepository nonConformityRepository, IInstrumentRepository instrumentRepository, ICalibrationRepository calibrationRepository, IUserRepository userRepository)
        {
            this.nonConformityRepository = nonConformityRepository;
            this.instrumentRepository = instrumentRepository;
            this.calibrationRepository = calibrationRepository;
            this.userRepository = userRepository;
        }

        public async Task<NonConformityDTO> AddAsync(NonConformityDTO dto)
        {
            await EnsureInstrumentExistsAsync(dto.InstrumentId);
            await EnsureCalibrationExistsAsync(dto.CalibrationId, dto.InstrumentId);
            await EnsureUserExistsAsync(dto.DetectedByUserId);

            // Si no llega código lo genera el servidor (NC-0001, NC-0002...); si llega uno, se respeta.
            var code = string.IsNullOrWhiteSpace(dto.Code) ? await GenerateCodeAsync() : dto.Code.Trim();
            await EnsureCodeIsUniqueAsync(code, 0);

            var createdAt = DateTime.Now;
            var origin = ParseEnum<Origin>(dto.Origin, nameof(dto.Origin));
            NonConformity nonConformity = new NonConformity(0, code, dto.Description, origin, NonConformityStatus.Open, dto.CorrectiveAction, createdAt, null, createdAt, dto.InstrumentId, dto.CalibrationId, dto.DetectedByUserId, null);

            await nonConformityRepository.AddAsync(nonConformity);

            dto.Id = nonConformity.Id;
            dto.Code = nonConformity.Code;
            dto.Status = nonConformity.Status.ToString();
            dto.OpenedAt = nonConformity.OpenedAt;
            dto.ClosedAt = null;
            dto.ClosedByUserId = null;
            dto.CreatedAt = nonConformity.CreatedAt;
            FillNavigationNames(dto, nonConformity);

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await nonConformityRepository.DeleteAsync(id);
        }

        public async Task<NonConformityDTO?> GetAsync(int id)
        {
            NonConformity? nonConformity = await nonConformityRepository.GetAsync(id);

            if (nonConformity == null)
                return null;

            return MapToDto(nonConformity);
        }

        public async Task<IEnumerable<NonConformityDTO>> GetAllAsync()
        {
            var nonConformities = await nonConformityRepository.GetAllAsync();
            return nonConformities.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(NonConformityDTO dto)
        {
            var existing = await nonConformityRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            await EnsureInstrumentExistsAsync(dto.InstrumentId);
            await EnsureCalibrationExistsAsync(dto.CalibrationId, dto.InstrumentId);
            await EnsureUserExistsAsync(dto.DetectedByUserId);
            await EnsureUserExistsAsync(dto.ClosedByUserId);
            await EnsureCodeIsUniqueAsync(dto.Code, dto.Id);

            var origin = ParseEnum<Origin>(dto.Origin, nameof(dto.Origin));
            var status = ParseEnum<NonConformityStatus>(dto.Status, nameof(dto.Status));

            // Cerrada <=> tiene ClosedBy. Al reabrir (ClosedBy null) se limpia ClosedAt y el estado vuelve a Open
            DateTime? closedAt;
            if (dto.ClosedByUserId.HasValue)
            {
                status = NonConformityStatus.Closed;
                closedAt = dto.ClosedAt ?? DateTime.Now;
            }
            else
            {
                closedAt = null;
                if (status == NonConformityStatus.Closed)
                    status = NonConformityStatus.Open;
            }

            NonConformity nonConformity = new NonConformity(dto.Id, dto.Code, dto.Description, origin, status, dto.CorrectiveAction, dto.OpenedAt, closedAt, existing.CreatedAt, dto.InstrumentId, dto.CalibrationId, dto.DetectedByUserId, dto.ClosedByUserId);
            return await nonConformityRepository.UpdateAsync(nonConformity);
        }

        public async Task<bool> CloseAsync(int id, int closerUserId)
        {
            var existing = await nonConformityRepository.GetAsync(id);
            if (existing == null)
                return false;

            if (existing.ClosedByUserId.HasValue)
                throw new InvalidOperationException("The non conformity is already closed.");

            await EnsureUserExistsAsync(closerUserId);

            NonConformity nonConformity = new NonConformity(existing.Id, existing.Code, existing.Description, existing.Origin, NonConformityStatus.Closed, existing.CorrectiveAction, existing.OpenedAt, DateTime.Now, existing.CreatedAt, existing.InstrumentId, existing.CalibrationId, existing.DetectedByUserId, closerUserId);
            return await nonConformityRepository.UpdateAsync(nonConformity);
        }

        private async Task EnsureInstrumentExistsAsync(int instrumentId)
        {
            var instrument = await instrumentRepository.GetAsync(instrumentId);
            if (instrument == null)
                throw new KeyNotFoundException($"There is no instrument with id {instrumentId}.");
        }

        // El siguiente número después del más alto que ya exista con formato NC-<número>.
        private async Task<string> GenerateCodeAsync()
        {
            var nonConformities = await nonConformityRepository.GetAllAsync();

            int highest = 0;
            foreach (var nonConformity in nonConformities)
            {
                var match = Regex.Match(nonConformity.Code ?? string.Empty, @"^NC-(\d+)$", RegexOptions.IgnoreCase);
                if (match.Success && int.TryParse(match.Groups[1].Value, out var number) && number > highest)
                    highest = number;
            }

            return $"NC-{highest + 1:D4}";
        }

        private async Task EnsureCodeIsUniqueAsync(string? code, int currentId)
        {
            var normalizedCode = (code ?? string.Empty).Trim();

            var nonConformities = await nonConformityRepository.GetAllAsync();
            if (nonConformities.Any(n => n.Id != currentId && string.Equals(n.Code.Trim(), normalizedCode, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Ya existe una no conformidad con el código '{normalizedCode}'.");
        }

        private async Task EnsureCalibrationExistsAsync(int? calibrationId, int instrumentId)
        {
            if (calibrationId == null)
                return;
            var calibration = await calibrationRepository.GetAsync(calibrationId.Value);
            if (calibration == null)
                throw new KeyNotFoundException($"There is no calibration with id {calibrationId}.");
            if (calibration.InstrumentId != instrumentId)
                throw new ArgumentException("The calibration does not belong to the selected instrument.", nameof(calibrationId));
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
                throw new ArgumentException($"The value '{value}' is not valid.", paramName);
            return parsed;
        }

        private static void FillNavigationNames(NonConformityDTO dto, NonConformity nonConformity)
        {
            dto.InstrumentCode = nonConformity.Instrument?.Code;
            dto.CalibrationCertificateNumber = nonConformity.Calibration?.CertificateNumber;
            dto.DetectedByUserName = nonConformity.DetectedByUser?.FullName;
            dto.ClosedByUserName = nonConformity.ClosedByUser?.FullName;
        }

        private static NonConformityDTO MapToDto(NonConformity nonConformity)
        {
            var dto = new NonConformityDTO
            {
                Id = nonConformity.Id,
                Code = nonConformity.Code,
                Description = nonConformity.Description,
                Origin = nonConformity.Origin.ToString(),
                Status = nonConformity.Status.ToString(),
                CorrectiveAction = nonConformity.CorrectiveAction,
                OpenedAt = nonConformity.OpenedAt,
                ClosedAt = nonConformity.ClosedAt,
                CreatedAt = nonConformity.CreatedAt,
                InstrumentId = nonConformity.InstrumentId,
                CalibrationId = nonConformity.CalibrationId,
                DetectedByUserId = nonConformity.DetectedByUserId,
                ClosedByUserId = nonConformity.ClosedByUserId
            };
            FillNavigationNames(dto, nonConformity);
            return dto;
        }
    }
}