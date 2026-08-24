using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CalibrationMeasurementRepository : ICalibrationMeasurementRepository
    {
        private readonly CalibrArContext context;

        public CalibrationMeasurementRepository(CalibrArContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(CalibrationMeasurement calibrationMeasurement)
        {
            context.CalibrationMeasurements.Add(calibrationMeasurement);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var calibrationMeasurement = await context.CalibrationMeasurements.FirstOrDefaultAsync(c => c.Id == id);
            if (calibrationMeasurement != null)
            {
                context.CalibrationMeasurements.Remove(calibrationMeasurement);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CalibrationMeasurement?> GetAsync(int id)
        {
            return await context.CalibrationMeasurements.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<CalibrationMeasurement>> GetAllAsync()
        {
            return await context.CalibrationMeasurements.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<bool> UpdateAsync(CalibrationMeasurement calibrationMeasurement)
        {
            var existing = await context.CalibrationMeasurements.FindAsync(calibrationMeasurement.Id);
            if (existing != null)
            {
                existing.SetNominalValue(calibrationMeasurement.NominalValue);
                existing.SetMeasuredValue(calibrationMeasurement.MeasuredValue);
                existing.SetError(calibrationMeasurement.Error);
                existing.SetIsWithinTolerance(calibrationMeasurement.IsWithinTolerance);
                existing.SetNotes(calibrationMeasurement.Notes);
                existing.SetCalibrationId(calibrationMeasurement.CalibrationId);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}