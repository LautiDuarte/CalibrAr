using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public class AreaRepository : IAreaRepository
    {
        private static readonly List<Area> areas = new List<Area>();
        private static int nextId = 1;

        public Task AddAsync(Area area)
        {
            area.SetId(nextId++);

            var locationRepo = new LocationRepository();
            var location = locationRepo.GetAllAsync().Result.FirstOrDefault(l => l.Id == area.LocationId);
            if (location != null)
                area.SetLocation(location);

            areas.Add(area);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var area = areas.FirstOrDefault(a => a.Id == id);
            if (area != null)
            {
                areas.Remove(area);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Area?> GetAsync(int id)
        {
            return Task.FromResult(areas.FirstOrDefault(a => a.Id == id));
        }

        public Task<IEnumerable<Area>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Area>>(areas.OrderBy(a => a.Name).ToList());
        }

        public Task<bool> UpdateAsync(Area area)
        {
            var existing = areas.FirstOrDefault(a => a.Id == area.Id);
            if (existing != null)
            {
                existing.SetName(area.Name);
                existing.SetResponsible(area.Responsible);
                existing.SetIsActive(area.IsActive);
                existing.SetCreatedAt(area.CreatedAt);
                existing.SetLocationId(area.LocationId);
                var locationRepo = new LocationRepository();
                var location = locationRepo.GetAllAsync().Result.FirstOrDefault(l => l.Id == area.LocationId);
                if (location != null)
                    existing.SetLocation(location);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

    }
}
