using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface IAreaRepository
    {
        Task AddAsync(Area area);
        Task<bool> DeleteAsync(int id);
        Task<Area?> GetAsync(int id);
        Task<IEnumerable<Area>> GetAllAsync();
        Task<bool> UpdateAsync(Area area);
    }
}
