using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface ICalibrationMeasurementRepository
    {
        Task AddAsync(CalibrationMeasurement calibrationMeasurement);
        Task<bool> DeleteAsync(int id);
        Task<CalibrationMeasurement?> GetAsync(int id);
        Task<IEnumerable<CalibrationMeasurement>> GetAllAsync();
        Task<bool> UpdateAsync(CalibrationMeasurement calibrationMeasurement);
    }
}