using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using Data;
using Domain.Model;

namespace Application.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository areaRepository;
        private readonly ILocationRepository locationRepository;

        public AreaService(IAreaRepository areaRepository, ILocationRepository locationRepository)
        {
            this.areaRepository = areaRepository;
            this.locationRepository = locationRepository;
        }

        public async Task<AreaDTO> AddAsync(AreaDTO dto)
        {
            await EnsureLocationExistsAsync(dto.LocationId);

            var createdAt = DateTime.Now;
            var area = new Area(0, dto.Name, dto.Responsible, dto.IsActive, createdAt, dto.LocationId);
            await areaRepository.AddAsync(area);
            dto.Id = area.Id;
            dto.CreatedAt = area.CreatedAt;
            dto.LocationName = area.Location?.Name;
            dto.LocationAddress = area.Location?.Address;
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await areaRepository.DeleteAsync(id);
        }

        public async Task<AreaDTO?> GetAsync(int id)
        {
            var area = await areaRepository.GetAsync(id);
            if (area == null)
                return null;
            return new AreaDTO
            {
                Id = area.Id,
                Name = area.Name,
                Responsible = area.Responsible,
                IsActive = area.IsActive,
                CreatedAt = area.CreatedAt,
                LocationId = area.LocationId,
                LocationName = area.Location?.Name,
                LocationAddress = area.Location?.Address
            };
        }

        public async Task<IEnumerable<AreaDTO>> GetAllAsync()
        {
            var areas = await areaRepository.GetAllAsync();
            return areas.Select(area => new AreaDTO
            {
                Id = area.Id,
                Name = area.Name,
                Responsible = area.Responsible,
                IsActive = area.IsActive,
                CreatedAt = area.CreatedAt,
                LocationId = area.LocationId,
                LocationName = area.Location?.Name,
                LocationAddress = area.Location?.Address
            });
        }

        public async Task<bool> UpdateAsync(AreaDTO dto)
        {
            var existing = await areaRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            await EnsureLocationExistsAsync(dto.LocationId);

            Area area = new Area(dto.Id, dto.Name, dto.Responsible, dto.IsActive, existing.CreatedAt, dto.LocationId);
            return await areaRepository.UpdateAsync(area);
        }

        private async Task EnsureLocationExistsAsync(int locationId)
        {
            var location = await locationRepository.GetAsync(locationId);
            if (location == null)
                throw new KeyNotFoundException($"No existe una Location con Id {locationId}.");
        }
    }
}
