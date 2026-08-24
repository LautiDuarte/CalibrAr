using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Application.Services
{
    public interface IInstrumentTypeService
    {
        Task<InstrumentTypeDTO> AddAsync(InstrumentTypeDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<InstrumentTypeDTO?> GetAsync(int id);
        Task<IEnumerable<InstrumentTypeDTO>> GetAllAsync();
        Task<bool> UpdateAsync(InstrumentTypeDTO dto);
    }
}