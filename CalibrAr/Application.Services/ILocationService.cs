using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Application.Services
{
    public interface ILocationService
    {
        Task<LocationDTO> AddAsync(LocationDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<LocationDTO?> GetAsync(int id);
        Task<IEnumerable<LocationDTO>> GetAllAsync();
        Task<bool> UpdateAsync(LocationDTO dto);
    }
}
