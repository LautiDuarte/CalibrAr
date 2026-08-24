using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Application.Services
{
    public interface IReferenceStandardService
    {
        Task<ReferenceStandardDTO> AddAsync(ReferenceStandardDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<ReferenceStandardDTO?> GetAsync(int id);
        Task<IEnumerable<ReferenceStandardDTO>> GetAllAsync();
        Task<bool> UpdateAsync(ReferenceStandardDTO dto);
    }
}