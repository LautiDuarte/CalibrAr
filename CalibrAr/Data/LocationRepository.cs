using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class LocationRepository : ILocationRepository
    {
        private readonly CalibrArContext context;

        public LocationRepository(CalibrArContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Location location)
        {
            context.Locations.Add(location);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var location = await context.Locations.FirstOrDefaultAsync(l => l.Id == id);
            if (location != null)
            {
                context.Locations.Remove(location);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Location?> GetAsync(int id)
        {
            return await context.Locations.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Location>> GetAllAsync()
        {
            return await context.Locations.OrderBy(l => l.Name).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Location location)
        {
            var existingLocation = await context.Locations.FindAsync(location.Id);
            if (existingLocation != null)
            {
                existingLocation.SetName(location.Name);
                existingLocation.SetAddress(location.Address);
                existingLocation.SetIsActive(location.IsActive);
                existingLocation.SetCreatedAt(location.CreatedAt);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
