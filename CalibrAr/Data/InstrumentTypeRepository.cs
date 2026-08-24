using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class InstrumentTypeRepository : IInstrumentTypeRepository
    {
        private readonly CalibrArContext context;
        public InstrumentTypeRepository(CalibrArContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(InstrumentType instrumentType)
        {
            context.InstrumentTypes.Add(instrumentType);
            await context.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var instrumentType = await context.InstrumentTypes.FirstOrDefaultAsync(i => i.Id == id);
            if (instrumentType != null)
            {
                context.InstrumentTypes.Remove(instrumentType);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<InstrumentType?> GetAsync(int id)
        {
            return await context.InstrumentTypes.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<IEnumerable<InstrumentType>> GetAllAsync()
        {
            return await context.InstrumentTypes.OrderBy(i => i.Name).ToListAsync();
        }
        public async Task<bool> UpdateAsync(InstrumentType instrumentType)
        {
            var existingInstrumentType = await context.InstrumentTypes.FindAsync(instrumentType.Id);
            if (existingInstrumentType != null)
            {
                existingInstrumentType.SetName(instrumentType.Name);
                existingInstrumentType.SetDescription(instrumentType.Description);
                existingInstrumentType.SetMeasurementUnit(instrumentType.MeasurementUnit);
                existingInstrumentType.SetMaxAllowedError(instrumentType.MaxAllowedError);
                existingInstrumentType.SetCalibrationFrequencyMonths(instrumentType.CalibrationFrequencyMonths);
                existingInstrumentType.SetIsActive(instrumentType.IsActive);
                existingInstrumentType.SetCreatedAt(instrumentType.CreatedAt);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
