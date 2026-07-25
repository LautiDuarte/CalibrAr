using Domain.Model;

namespace Data
{
    public class LocationRepository : ILocationRepository
    {
        private static readonly List<Location> locations = new List<Location>
        {
            new Location(1, "Rosario Sur", "Laprida 3568", true, DateTime.Now),
            new Location(2, "Rosario Norte", "Bv. Rondeau 894", true, DateTime.Now)
        };

        public Task AddAsync(Location location)
        {
            location.SetId(locations.Count + 1);

            locations.Add(location);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var location = locations.FirstOrDefault(l => l.Id == id);
            if (location != null)
            {
                locations.Remove(location);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Location?> GetAsync(int id)
        {
            return Task.FromResult(locations.FirstOrDefault(l => l.Id == id));
        }

        public Task<IEnumerable<Location>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Location>>(locations.OrderBy(p => p.Name).ToList());
        }

        public Task<bool> UpdateAsync(Location location)
        {
            var existing = locations.FirstOrDefault(l => l.Id == location.Id);
            if (existing != null)
            {
                existing.SetName(location.Name);
                existing.SetAddress(location.Address);
                existing.SetIsActive(location.IsActive);
                existing.SetCreatedAt(location.CreatedAt);

                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
