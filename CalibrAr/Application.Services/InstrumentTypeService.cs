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
    public class InstrumentTypeService : IInstrumentTypeService
    {
        private readonly IInstrumentTypeRepository instrumentTypeRepository;

        public InstrumentTypeService(IInstrumentTypeRepository instrumentTypeRepository)
        {
            this.instrumentTypeRepository = instrumentTypeRepository;
        }

        public async Task<InstrumentTypeDTO> AddAsync(InstrumentTypeDTO dto)
        {
            var createdAt = DateTime.Now;
            InstrumentType instrumentType = new InstrumentType(0, dto.Name, dto.Description, dto.MeasurementUnit, dto.MaxAllowedError, dto.CalibrationFrequencyMonths, dto.IsActive, createdAt);

            await instrumentTypeRepository.AddAsync(instrumentType);

            dto.Id = instrumentType.Id;
            dto.CreatedAt = instrumentType.CreatedAt;

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await instrumentTypeRepository.DeleteAsync(id);
        }

        public async Task<InstrumentTypeDTO?> GetAsync(int id)
        {
            InstrumentType? instrumentType = await instrumentTypeRepository.GetAsync(id);

            if (instrumentType == null)
                return null;

            return new InstrumentTypeDTO
            {
                Id = instrumentType.Id,
                Name = instrumentType.Name,
                Description = instrumentType.Description,
                MeasurementUnit = instrumentType.MeasurementUnit,
                MaxAllowedError = instrumentType.MaxAllowedError,
                CalibrationFrequencyMonths = instrumentType.CalibrationFrequencyMonths,
                IsActive = instrumentType.IsActive,
                CreatedAt = instrumentType.CreatedAt
            };
        }

        public async Task<IEnumerable<InstrumentTypeDTO>> GetAllAsync()
        {
            var instrumentTypes = await instrumentTypeRepository.GetAllAsync();
            return instrumentTypes.Select(instrumentType => new InstrumentTypeDTO
            {
                Id = instrumentType.Id,
                Name = instrumentType.Name,
                Description = instrumentType.Description,
                MeasurementUnit = instrumentType.MeasurementUnit,
                MaxAllowedError = instrumentType.MaxAllowedError,
                CalibrationFrequencyMonths = instrumentType.CalibrationFrequencyMonths,
                IsActive = instrumentType.IsActive,
                CreatedAt = instrumentType.CreatedAt
            });
        }

        public async Task<bool> UpdateAsync(InstrumentTypeDTO dto)
        {
            var existing = await instrumentTypeRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            InstrumentType instrumentType = new InstrumentType(dto.Id, dto.Name, dto.Description, dto.MeasurementUnit, dto.MaxAllowedError, dto.CalibrationFrequencyMonths, dto.IsActive, existing.CreatedAt);
            return await instrumentTypeRepository.UpdateAsync(instrumentType);
        }
    }
}