using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Application.Services
{
    public interface IProcedureService
    {
        Task<ProcedureDTO> AddAsync(ProcedureDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<ProcedureDTO?> GetAsync(int id);
        Task<IEnumerable<ProcedureDTO>> GetAllAsync();
        Task<bool> UpdateAsync(ProcedureDTO dto);
    }
}