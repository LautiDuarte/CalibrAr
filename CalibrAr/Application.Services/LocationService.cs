using Data;
using DTOs;
using Domain.Model;

namespace Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository locationRepository;
        private readonly IAreaRepository areaRepository;

        public LocationService(ILocationRepository locationRepository, IAreaRepository areaRepository)
        {
            this.locationRepository = locationRepository;
            this.areaRepository = areaRepository;
        }

        public async Task<LocationDTO> AddAsync(LocationDTO dto)
        {
            var createdAt = DateTime.Now;
            Location location = new Location(0, dto.Name, dto.Address, dto.IsActive, createdAt);

            await locationRepository.AddAsync(location);

            dto.Id = location.Id;
            dto.CreatedAt = location.CreatedAt;

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var areas = await areaRepository.GetAllAsync();
            if (areas.Any(a => a.LocationId == id))
                throw new InvalidOperationException($"No se puede eliminar la Location {id} porque está en uso por una o más Areas.");

            return await locationRepository.DeleteAsync(id);
        }

        public async Task<LocationDTO?> GetAsync(int id)
        {
            Location? location = await locationRepository.GetAsync(id);

            if (location == null)
                return null;

            return new LocationDTO
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
                IsActive = location.IsActive,
                CreatedAt = location.CreatedAt
            };
        }

        public async Task<IEnumerable<LocationDTO>> GetAllAsync()
        {
           var locations = await locationRepository.GetAllAsync();
            return locations.Select(location => new LocationDTO
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
                IsActive = location.IsActive,
                CreatedAt = location.CreatedAt
            });
        }

        public async Task<bool> UpdateAsync(LocationDTO dto)
        {
            var existing = await locationRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            Location location = new Location(dto.Id, dto.Name, dto.Address, dto.IsActive, existing.CreatedAt);
            return await locationRepository.UpdateAsync(location);
        }
    }
}
