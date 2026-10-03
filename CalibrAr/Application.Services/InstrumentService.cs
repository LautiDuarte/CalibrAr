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
    public class InstrumentService : IInstrumentService
    {
        private readonly IInstrumentRepository instrumentRepository;
        private readonly IInstrumentTypeRepository instrumentTypeRepository;
        private readonly IAreaRepository areaRepository;

        public InstrumentService(IInstrumentRepository instrumentRepository, IInstrumentTypeRepository instrumentTypeRepository, IAreaRepository areaRepository)
        {
            this.instrumentRepository = instrumentRepository;
            this.instrumentTypeRepository = instrumentTypeRepository;
            this.areaRepository = areaRepository;
        }

        public async Task<InstrumentDTO> AddAsync(InstrumentDTO dto)
        {
            await EnsureInstrumentTypeExistsAsync(dto.InstrumentTypeId);
            await EnsureAreaExistsAsync(dto.AreaId);

            var createdAt = DateTime.Now;
            var status = InstrumentStatus.Active; 
            var isActive = true;
            Instrument instrument = new Instrument(0, dto.Code, dto.Name, dto.SerialNumber, dto.Brand, dto.Model, status, dto.MaxAllowedError, dto.CalibrationFrequencyMonths, dto.LastCalibrationDate, dto.NextCalibrationDate, isActive, createdAt, null, dto.InstrumentTypeId, dto.AreaId);

            await instrumentRepository.AddAsync(instrument);

            dto.Id = instrument.Id;
            dto.CreatedAt = instrument.CreatedAt;
            dto.UpdatedAt = instrument.UpdatedAt;
            dto.InstrumentTypeName = instrument.InstrumentType?.Name;
            dto.AreaName = instrument.Area?.Name;
            dto.IsActive = isActive;

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await instrumentRepository.DeleteAsync(id);
        }

        public async Task<InstrumentDTO?> GetAsync(int id)
        {
            Instrument? instrument = await instrumentRepository.GetAsync(id);

            if (instrument == null)
                return null;

            return MapToDto(instrument);
        }

        public async Task<IEnumerable<InstrumentDTO>> GetAllAsync()
        {
            var instruments = await instrumentRepository.GetAllAsync();
            if (instruments == null || !instruments.Any())
                return Enumerable.Empty<InstrumentDTO>();
            var instrumentsUpToDate = await CheckCalibrationExpiredAsync(instruments);
            return instrumentsUpToDate.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(InstrumentDTO dto)
        {
            var existing = await instrumentRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;


            await EnsureInstrumentTypeExistsAsync(dto.InstrumentTypeId);
            await EnsureAreaExistsAsync(dto.AreaId);

            var status = ParseStatus(dto.Status);
            Instrument instrument = new Instrument(dto.Id, dto.Code, dto.Name, dto.SerialNumber, dto.Brand, dto.Model, status, dto.MaxAllowedError, dto.CalibrationFrequencyMonths, dto.LastCalibrationDate, dto.NextCalibrationDate, dto.IsActive, existing.CreatedAt, DateTime.Now, dto.InstrumentTypeId, dto.AreaId);
            Instrument instrumentUpToDate  = await IsActiveCheck(instrument);
            return await instrumentRepository.UpdateAsync(instrumentUpToDate);
        }

        public Task<Instrument> IsActiveCheck(Instrument instrument) // chequear funcionamiento de esta funcion
        {
            if (instrument.Status == InstrumentStatus.Decommissioned)
            {
                instrument.SetIsActive(false);
            }
            else
            {
                instrument.SetIsActive(true);
            }
            return Task.FromResult(instrument);
        }

        public async Task<IEnumerable<Instrument>> CheckCalibrationExpiredAsync(IEnumerable<Instrument> instruments) //chequear funcionamiento de esta funcion
        {
            foreach (var instrument in instruments)
            {
                if (instrument.Status == InstrumentStatus.Active && instrument.NextCalibrationDate.HasValue && instrument.NextCalibrationDate.Value < DateTime.Now)
                {
                    instrument.SetStatus(InstrumentStatus.CalibrationExpired);
                    InstrumentDTO instrumentDto = MapToDto(instrument);
                    await this.UpdateAsync(instrumentDto);
                }
            }
            return instruments;
        }

        private async Task EnsureInstrumentTypeExistsAsync(int instrumentTypeId)
        {
            var instrumentType = await instrumentTypeRepository.GetAsync(instrumentTypeId);
            if (instrumentType == null)
                throw new KeyNotFoundException($"There is no instrument type with id {instrumentTypeId}.");
        }

        private async Task EnsureAreaExistsAsync(int areaId)
        {
            var area = await areaRepository.GetAsync(areaId);
            if (area == null)
                throw new KeyNotFoundException($"There is no area with id {areaId}.");
        }

        private static InstrumentStatus ParseStatus(string status)
        {
            if (!Enum.TryParse<InstrumentStatus>(status, out var parsed))
                throw new ArgumentException($"The status '{status}' is not valid.", nameof(status));
            return parsed;
        }

        private static InstrumentDTO MapToDto(Instrument instrument)
        {
            return new InstrumentDTO
            {
                Id = instrument.Id,
                Code = instrument.Code,
                Name = instrument.Name,
                SerialNumber = instrument.SerialNumber,
                Brand = instrument.Brand,
                Model = instrument.Model,
                Status = instrument.Status.ToString(),
                MaxAllowedError = instrument.MaxAllowedError,
                CalibrationFrequencyMonths = instrument.CalibrationFrequencyMonths,
                LastCalibrationDate = instrument.LastCalibrationDate,
                NextCalibrationDate = instrument.NextCalibrationDate,
                IsActive = instrument.IsActive,
                CreatedAt = instrument.CreatedAt,
                UpdatedAt = instrument.UpdatedAt,
                InstrumentTypeId = instrument.InstrumentTypeId,
                InstrumentTypeName = instrument.InstrumentType?.Name,
                AreaId = instrument.AreaId,
                AreaName = instrument.Area?.Name
            };
        }
    }
}