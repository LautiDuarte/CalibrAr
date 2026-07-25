using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Application.Services
{
    public interface IAreaService
    {
        Task<AreaDTO> AddAsync(AreaDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<AreaDTO?> GetAsync(int id);
        Task<IEnumerable<AreaDTO>> GetAllAsync();
        Task<bool> UpdateAsync(AreaDTO dto);
    }
}
