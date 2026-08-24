using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Application.Services
{
    public interface ICalibrationService
    {
        Task<CalibrationDTO> AddAsync(CalibrationDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<CalibrationDTO?> GetAsync(int id);
        Task<IEnumerable<CalibrationDTO>> GetAllAsync();
        Task<bool> UpdateAsync(CalibrationDTO dto);
    }
}