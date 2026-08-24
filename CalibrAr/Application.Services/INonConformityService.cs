using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Application.Services
{
    public interface INonConformityService
    {
        Task<NonConformityDTO> AddAsync(NonConformityDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<NonConformityDTO?> GetAsync(int id);
        Task<IEnumerable<NonConformityDTO>> GetAllAsync();
        Task<bool> UpdateAsync(NonConformityDTO dto);
    }
}