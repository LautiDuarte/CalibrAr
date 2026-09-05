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
            await EnsureCalibrationExistsAsync(dto.CalibrationId);
            await EnsureUserExistsAsync(dto.DetectedByUserId);
            await EnsureUserExistsAsync(dto.ClosedByUserId);

            var createdAt = DateTime.Now;
            var origin = ParseEnum<Origin>(dto.Origin, nameof(dto.Origin));
            var status = ParseEnum<NonConformityStatus>(dto.Status, nameof(dto.Status));
            NonConformity nonConformity = new NonConformity(0, dto.Code, dto.Description, origin, status, dto.CorrectiveAction, dto.OpenedAt, dto.ClosedAt, createdAt, dto.InstrumentId, dto.CalibrationId, dto.DetectedByUserId, dto.ClosedByUserId);

            await nonConformityRepository.AddAsync(nonConformity);

            dto.Id = nonConformity.Id;
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
            await EnsureCalibrationExistsAsync(dto.CalibrationId);
            await EnsureUserExistsAsync(dto.DetectedByUserId);
            await EnsureUserExistsAsync(dto.ClosedByUserId);

            var origin = ParseEnum<Origin>(dto.Origin, nameof(dto.Origin));
            var status = ParseEnum<NonConformityStatus>(dto.Status, nameof(dto.Status));
            NonConformity nonConformity = new NonConformity(dto.Id, dto.Code, dto.Description, origin, status, dto.CorrectiveAction, dto.OpenedAt, dto.ClosedAt, existing.CreatedAt, dto.InstrumentId, dto.CalibrationId, dto.DetectedByUserId, dto.ClosedByUserId);
            return await nonConformityRepository.UpdateAsync(nonConformity);
        }

        private async Task EnsureInstrumentExistsAsync(int instrumentId)
        {
            var instrument = await instrumentRepository.GetAsync(instrumentId);
            if (instrument == null)
                throw new KeyNotFoundException($"There is no instrument with id {instrumentId}.");
        }

        private async Task EnsureCalibrationExistsAsync(int calibrationId)
        {
            var calibration = await calibrationRepository.GetAsync(calibrationId);
            if (calibration == null)
                throw new KeyNotFoundException($"There is no calibration with id {calibrationId}.");
        }

        private async Task EnsureUserExistsAsync(int userId)
        {
            var user = await userRepository.GetAsync(userId);
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