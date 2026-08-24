using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AreaRepository : IAreaRepository
    {
        private readonly CalibrArContext context;
        public AreaRepository(CalibrArContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Area area)
        {
            context.Areas.Add(area);
            await context.SaveChangesAsync();
            await context.Entry(area).Reference(a => a.Location).LoadAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var area = await context.Areas.FirstOrDefaultAsync(a => a.Id == id);
            if (area != null)
            {
                context.Areas.Remove(area);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Area?> GetAsync(int id)
        {
            return await context.Areas.Include(a => a.Location).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Area>> GetAllAsync()
        {
            return await context.Areas.Include(a => a.Location).OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Area area)
        {
            var existingArea = await context.Areas.FindAsync(area.Id);
            if (existingArea != null)
            {
                existingArea.SetName(area.Name);
                existingArea.SetResponsible(area.Responsible);
                existingArea.SetIsActive(area.IsActive);
                existingArea.SetCreatedAt(area.CreatedAt);
                existingArea.SetLocationId(area.LocationId);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
