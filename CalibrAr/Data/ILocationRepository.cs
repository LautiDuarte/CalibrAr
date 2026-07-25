using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface ILocationRepository
    {
        Task AddAsync(Location location);
        Task<bool> DeleteAsync(int id);
        Task<Location?> GetAsync(int id);
        Task<IEnumerable<Location>> GetAllAsync();
        Task<bool> UpdateAsync(Location location);
    }
}
