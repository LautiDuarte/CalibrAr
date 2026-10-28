using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using DTOs;
using Domain.Model;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Application.Services
{
    public class InstrumentService : IInstrumentService
    {
        private readonly IInstrumentRepository instrumentRepository;
        private readonly IInstrumentTypeRepository instrumentTypeRepository;
        private readonly IInstrumentStatusHistoryRepository instrumentStatusHistoryRepository;
        private readonly IAreaRepository areaRepository;
        private readonly ISearchRepository searchRepository;

        public InstrumentService(IInstrumentRepository instrumentRepository, IInstrumentTypeRepository instrumentTypeRepository, IAreaRepository areaRepository, IInstrumentStatusHistoryRepository instrumentStatusHistoryRepository, ISearchRepository searchRepository)
        {
            this.searchRepository = searchRepository;
            this.instrumentRepository = instrumentRepository;
            this.instrumentTypeRepository = instrumentTypeRepository;
            this.areaRepository = areaRepository;
            this.instrumentStatusHistoryRepository = instrumentStatusHistoryRepository;
        }

        public async Task<InstrumentDTO> AddAsync(InstrumentDTO dto)
        {
            await EnsureCodeIsUniqueAsync(dto.Code);
            await EnsureInstrumentTypeExistsAsync(dto.InstrumentTypeId);
            await EnsureAreaExistsAsync(dto.AreaId);

            // La frecuencia se guarda tal cual: null = hereda del tipo
            var effectiveFrequency = await ResolveFrequencyMonthsAsync(dto.CalibrationFrequencyMonths, dto.InstrumentTypeId);
            var nextCalibrationDate = Instrument.CalculateNextCalibrationDate(dto.LastCalibrationDate, effectiveFrequency);

            var createdAt = DateTime.Now;
            var status = InstrumentStatus.Active;
            var isActive = true;
            Instrument instrument = new Instrument(0, dto.Code, dto.Name, dto.SerialNumber, dto.Brand, dto.Model, status, dto.MaxAllowedError, dto.CalibrationFrequencyMonths, dto.LastCalibrationDate, nextCalibrationDate, isActive, createdAt, null, dto.InstrumentTypeId, dto.AreaId);

            await instrumentRepository.AddAsync(instrument);

            dto.Id = instrument.Id;
            dto.CreatedAt = instrument.CreatedAt;
            dto.UpdatedAt = instrument.UpdatedAt;
            dto.NextCalibrationDate = nextCalibrationDate;
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

        // Los filtros se resuelven en la base (SearchRepository, ADO.NET); acá solo se cargan y mapean los resultados
        public async Task<IEnumerable<InstrumentDTO>> SearchAsync(string? text, string? status, int? areaId, bool hideDecommissioned)
        {
            InstrumentStatus? parsedStatus = string.IsNullOrWhiteSpace(status) ? null : ParseStatus(status);

            var ids = await searchRepository.SearchInstrumentIdsAsync(text, parsedStatus, areaId, hideDecommissioned);
            if (ids.Count == 0)
                return Enumerable.Empty<InstrumentDTO>();

            var instruments = await instrumentRepository.GetByIdsAsync(ids);
            var instrumentsUpToDate = await CheckCalibrationExpiredAsync(instruments);
            return instrumentsUpToDate.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(InstrumentDTO dto, int? changedByUserId = null)
        {
            var existing = await instrumentRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            var previousStatus = existing.Status;

            if (dto.InstrumentTypeId != existing.InstrumentTypeId)
                throw new InvalidOperationException("El tipo de instrumento no se puede cambiar después de crearlo.");

            await EnsureCodeIsUniqueAsync(dto.Code, dto.Id);
            await EnsureInstrumentTypeExistsAsync(dto.InstrumentTypeId);
            await EnsureAreaExistsAsync(dto.AreaId);

            // La fuente de verdad de la �ltima fecha son las calibraciones (existing), no el DTO
            var lastCalibrationDate = existing.LastCalibrationDate;
            var effectiveFrequency = await ResolveFrequencyMonthsAsync(dto.CalibrationFrequencyMonths, dto.InstrumentTypeId);
            var nextCalibrationDate = Instrument.CalculateNextCalibrationDate(lastCalibrationDate, effectiveFrequency);

            var status = ParseStatus(dto.Status);
            if (status == InstrumentStatus.CalibrationExpired
                && (!nextCalibrationDate.HasValue
                || nextCalibrationDate.Value >= DateTime.Now))
            {
                status = InstrumentStatus.Active; // al alargar la frecuencia deja de estar vencido
            }

            Instrument instrument = new Instrument(dto.Id, dto.Code, dto.Name, dto.SerialNumber, dto.Brand, dto.Model, status, dto.MaxAllowedError, dto.CalibrationFrequencyMonths, lastCalibrationDate, nextCalibrationDate, dto.IsActive, existing.CreatedAt, DateTime.Now, dto.InstrumentTypeId, dto.AreaId);
            Instrument instrumentUpToDate = await IsActiveCheck(instrument);
            var updated = await instrumentRepository.UpdateAsync(instrumentUpToDate);
            if (updated && previousStatus != status)
                await RegisterStatusChangeAsync(dto.Id, previousStatus, status, changedByUserId);
            return updated;
        }

        // Lo llama CalibrationService cuando cambia la �ltima calibraci�n del instrumento
        public async Task RecalculateCalibrationScheduleAsync(int instrumentId, DateTime? lastCalibrationDate = null)
        {
            var instrument = await instrumentRepository.GetAsync(instrumentId);
            if (instrument == null)
                return;
            var previousStatus = instrument.Status;
            var effectiveFrequency = await ResolveFrequencyMonthsAsync(instrument.CalibrationFrequencyMonths, instrument.InstrumentTypeId);
            var nextCalibrationDate = (DateTime?)null;
            if (lastCalibrationDate == null)
            {
                nextCalibrationDate = null;
            }
            else
            {
                nextCalibrationDate = Instrument.CalculateNextCalibrationDate(lastCalibrationDate, effectiveFrequency);
            }
            instrument.SetLastCalibrationDate(lastCalibrationDate);
            instrument.SetNextCalibrationDate(nextCalibrationDate);

            if (instrument.Status == InstrumentStatus.CalibrationExpired
            && (!nextCalibrationDate.HasValue || nextCalibrationDate.Value >= DateTime.Now))
            {
                instrument.SetStatus(InstrumentStatus.Active);
            }

            await instrumentRepository.UpdateAsync(instrument);
            if (previousStatus != instrument.Status)
                await RegisterStatusChangeAsync(instrument.Id, previousStatus, instrument.Status, null);
        }

        // Lo llama InstrumentTypeService cuando cambia la frecuencia del tipo
        public async Task RecalculateScheduleForTypeAsync(int instrumentTypeId)
        {
            var instruments = await instrumentRepository.GetAllAsync();
            var inheriting = instruments
                .Where(i => i.InstrumentTypeId == instrumentTypeId && i.CalibrationFrequencyMonths == null)
                .ToList();

            foreach (var instrument in inheriting)
                await RecalculateCalibrationScheduleAsync(instrument.Id, instrument.LastCalibrationDate);
        }

        // La frecuencia del instrumento sobreescribe la del tipo si est� definida
        private async Task<int?> ResolveFrequencyMonthsAsync(int? instrumentFrequencyMonths, int instrumentTypeId)
        {
            if (instrumentFrequencyMonths != null)
                return instrumentFrequencyMonths;

            var instrumentType = await instrumentTypeRepository.GetAsync(instrumentTypeId);
            return instrumentType?.CalibrationFrequencyMonths;
        }

        public Task<Instrument> IsActiveCheck(Instrument instrument)
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
                    var previousStatus = instrument.Status;
                    instrument.SetStatus(InstrumentStatus.CalibrationExpired);
                    InstrumentDTO instrumentDto = MapToDto(instrument);
                    await this.UpdateAsync(instrumentDto);
                    await RegisterStatusChangeAsync(instrument.Id, previousStatus, InstrumentStatus.CalibrationExpired, null);
                }
            }
            return instruments;
        }

        private async Task RegisterStatusChangeAsync(int instrumentId, InstrumentStatus previous, InstrumentStatus next, int? changedByUserId)
        {
            var history = new InstrumentStatusHistory(0, instrumentId, previous, next, changedByUserId, DateTime.Now);
            await instrumentStatusHistoryRepository.AddAsync(history);
        }

        private async Task EnsureInstrumentTypeExistsAsync(int instrumentTypeId)
        {
            if (instrumentTypeId <= 0)
                throw new ArgumentException("Debe seleccionar un tipo de instrumento.");

            var instrumentType = await instrumentTypeRepository.GetAsync(instrumentTypeId);
            if (instrumentType == null)
                throw new KeyNotFoundException($"No existe un tipo de instrumento con id {instrumentTypeId}.");
        }

        private async Task EnsureAreaExistsAsync(int areaId)
        {
            if (areaId <= 0)
                throw new ArgumentException("Debe seleccionar un área.");

            var area = await areaRepository.GetAsync(areaId);
            if (area == null)
                throw new KeyNotFoundException($"No existe un área con id {areaId}.");
        }

        private async Task EnsureCodeIsUniqueAsync(string code, int? excludeId = null)
        {
            var normalized = code?.Trim() ?? string.Empty;
            if (await instrumentRepository.ExistsByCodeAsync(normalized, excludeId))
                throw new InvalidOperationException($"Ya existe un instrumento con el código '{normalized}'.");
        }

        private static InstrumentStatus ParseStatus(string status)
        {
            if (!Enum.TryParse<InstrumentStatus>(status, out var parsed))
                throw new ArgumentException($"El estado '{status}' no es válido.");
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