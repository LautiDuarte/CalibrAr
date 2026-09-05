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
    public class InstrumentStatusHistoryService : IInstrumentStatusHistoryService
    {
        private readonly IInstrumentStatusHistoryRepository instrumentStatusHistoryRepository;
        private readonly IInstrumentRepository instrumentRepository;
        private readonly IUserRepository userRepository;

        public InstrumentStatusHistoryService(IInstrumentStatusHistoryRepository instrumentStatusHistoryRepository, IInstrumentRepository instrumentRepository, IUserRepository userRepository)
        {
            this.instrumentStatusHistoryRepository = instrumentStatusHistoryRepository;
            this.instrumentRepository = instrumentRepository;
            this.userRepository = userRepository;
        }

        public async Task<InstrumentStatusHistoryDTO> AddAsync(InstrumentStatusHistoryDTO dto)
        {
            await EnsureInstrumentExistsAsync(dto.InstrumentId);
            await EnsureUserExistsAsync(dto.ChangedByUserId);

            var changedAt = DateTime.Now;
            var previousStatus = ParseEnum<InstrumentStatus>(dto.PreviousStatus, nameof(dto.PreviousStatus));
            var newStatus = ParseEnum<InstrumentStatus>(dto.NewStatus, nameof(dto.NewStatus));
            InstrumentStatusHistory instrumentStatusHistory = new InstrumentStatusHistory(0, dto.InstrumentId, previousStatus, newStatus, dto.Reason, dto.ChangedByUserId, changedAt);

            await instrumentStatusHistoryRepository.AddAsync(instrumentStatusHistory);

            dto.Id = instrumentStatusHistory.Id;
            dto.ChangedAt = instrumentStatusHistory.ChangedAt;
            FillNavigationNames(dto, instrumentStatusHistory);

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await instrumentStatusHistoryRepository.DeleteAsync(id);
        }

        public async Task<InstrumentStatusHistoryDTO?> GetAsync(int id)
        {
            InstrumentStatusHistory? instrumentStatusHistory = await instrumentStatusHistoryRepository.GetAsync(id);

            if (instrumentStatusHistory == null)
                return null;

            return MapToDto(instrumentStatusHistory);
        }

        public async Task<IEnumerable<InstrumentStatusHistoryDTO>> GetAllAsync()
        {
            var instrumentStatusHistories = await instrumentStatusHistoryRepository.GetAllAsync();
            return instrumentStatusHistories.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(InstrumentStatusHistoryDTO dto)
        {
            var existing = await instrumentStatusHistoryRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            await EnsureInstrumentExistsAsync(dto.InstrumentId);
            await EnsureUserExistsAsync(dto.ChangedByUserId);

            var previousStatus = ParseEnum<InstrumentStatus>(dto.PreviousStatus, nameof(dto.PreviousStatus));
            var newStatus = ParseEnum<InstrumentStatus>(dto.NewStatus, nameof(dto.NewStatus));
            InstrumentStatusHistory instrumentStatusHistory = new InstrumentStatusHistory(dto.Id, dto.InstrumentId, previousStatus, newStatus, dto.Reason, dto.ChangedByUserId, existing.ChangedAt);
            return await instrumentStatusHistoryRepository.UpdateAsync(instrumentStatusHistory);
        }

        private async Task EnsureInstrumentExistsAsync(int instrumentId)
        {
            var instrument = await instrumentRepository.GetAsync(instrumentId);
            if (instrument == null)
                throw new KeyNotFoundException($"There is no instrument with id {instrumentId}.");
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

        private static void FillNavigationNames(InstrumentStatusHistoryDTO dto, InstrumentStatusHistory instrumentStatusHistory)
        {
            dto.InstrumentCode = instrumentStatusHistory.Instrument?.Code;
            dto.ChangedByUserName = instrumentStatusHistory.ChangedByUser?.FullName;
        }

        private static InstrumentStatusHistoryDTO MapToDto(InstrumentStatusHistory instrumentStatusHistory)
        {
            var dto = new InstrumentStatusHistoryDTO
            {
                Id = instrumentStatusHistory.Id,
                PreviousStatus = instrumentStatusHistory.PreviousStatus.ToString(),
                NewStatus = instrumentStatusHistory.NewStatus.ToString(),
                Reason = instrumentStatusHistory.Reason,
                ChangedAt = instrumentStatusHistory.ChangedAt,
                InstrumentId = instrumentStatusHistory.InstrumentId,
                ChangedByUserId = instrumentStatusHistory.ChangedByUserId
            };
            FillNavigationNames(dto, instrumentStatusHistory);
            return dto;
        }
    }
}