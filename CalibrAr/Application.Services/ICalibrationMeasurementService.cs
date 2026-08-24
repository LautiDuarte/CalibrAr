using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Application.Services
{
    public interface ICalibrationMeasurementService
    {
        Task<CalibrationMeasurementDTO> AddAsync(CalibrationMeasurementDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<CalibrationMeasurementDTO?> GetAsync(int id);
        Task<IEnumerable<CalibrationMeasurementDTO>> GetAllAsync();
        Task<bool> UpdateAsync(CalibrationMeasurementDTO dto);
    }
}