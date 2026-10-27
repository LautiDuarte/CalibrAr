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

        public InstrumentStatusHistoryService(IInstrumentStatusHistoryRepository instrumentStatusHistoryRepository)
        {
            this.instrumentStatusHistoryRepository = instrumentStatusHistoryRepository;
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
                ChangedAt = instrumentStatusHistory.ChangedAt,
                InstrumentId = instrumentStatusHistory.InstrumentId,
                ChangedByUserId = instrumentStatusHistory.ChangedByUserId
            };
            FillNavigationNames(dto, instrumentStatusHistory);
            return dto;
        }
    }
}