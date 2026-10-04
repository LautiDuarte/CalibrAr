using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface ICalibrationRepository
    {
        Task AddAsync(Calibration calibration);
        Task<bool> DeleteAsync(int id);
        Task<Calibration?> GetAsync(int id);
        Task<IEnumerable<Calibration>> GetAllAsync();
        Task<bool> UpdateAsync(Calibration calibration);
        Task<Calibration?> GetLatestByInstrumentAsync(int instrumentId);
    }
}