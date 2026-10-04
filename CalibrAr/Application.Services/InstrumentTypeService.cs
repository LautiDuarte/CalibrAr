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
        private readonly IInstrumentRepository instrumentRepository;
        private readonly IInstrumentService instrumentService;

        public InstrumentTypeService(IInstrumentTypeRepository instrumentTypeRepository, IInstrumentRepository instrumentRepository, IInstrumentService instrumentService)
        {
            this.instrumentTypeRepository = instrumentTypeRepository;
            this.instrumentRepository = instrumentRepository;
            this.instrumentService = instrumentService;
        }

        public async Task<InstrumentTypeDTO> AddAsync(InstrumentTypeDTO dto)
        {
            var createdAt = DateTime.Now;
            var isActive = true;
            InstrumentType instrumentType = new InstrumentType(0, dto.Name, dto.Description, dto.MeasurementUnit, dto.MaxAllowedError, dto.CalibrationFrequencyMonths, isActive, createdAt);

            await instrumentTypeRepository.AddAsync(instrumentType);

            dto.Id = instrumentType.Id;
            dto.CreatedAt = instrumentType.CreatedAt;
            dto.IsActive = instrumentType.IsActive;

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var instruments = await instrumentRepository.GetAllAsync();
            if (instruments.Any(i => i.InstrumentTypeId == id))
                throw new InvalidOperationException($"Cannot delete instrument type with id {id} because it is being used");

            return await instrumentTypeRepository.DeleteAsync(id);
        }

        public async Task<InstrumentTypeDTO?> GetAsync(int id)
        {
            InstrumentType? instrumentType = await instrumentTypeRepository.GetAsync(id);

            if (instrumentType == null)
                return null;

            return MapToDto(instrumentType);
        }

        public async Task<IEnumerable<InstrumentTypeDTO>> GetAllAsync()
        {
            var instrumentTypes = await instrumentTypeRepository.GetAllAsync();
            return instrumentTypes.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(InstrumentTypeDTO dto)
        {
            var existing = await instrumentTypeRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            // Capturar antes de guardar: el repositorio puede devolver la misma instancia trackeada
            var previousFrequencyMonths = existing.CalibrationFrequencyMonths;

            InstrumentType instrumentType = new InstrumentType(dto.Id, dto.Name, dto.Description, dto.MeasurementUnit, dto.MaxAllowedError, dto.CalibrationFrequencyMonths, dto.IsActive, existing.CreatedAt);
            var updated = await instrumentTypeRepository.UpdateAsync(instrumentType);
            if (!updated)
                return false;

            // Si cambió la frecuencia del tipo, los instrumentos que la heredan (frecuencia null)
            // tienen que recalcular su próxima fecha de calibración
            if (previousFrequencyMonths != dto.CalibrationFrequencyMonths)
                await instrumentService.RecalculateScheduleForTypeAsync(dto.Id);

            return true;
        }

        private static InstrumentTypeDTO MapToDto(InstrumentType instrumentType)
        {
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
    }
}