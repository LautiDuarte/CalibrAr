using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class InstrumentRepository : IInstrumentRepository
    {
        private readonly CalibrArContext context;

        public InstrumentRepository(CalibrArContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Instrument instrument)
        {
            context.Instruments.Add(instrument);
            await context.SaveChangesAsync();
            await context.Entry(instrument).Reference(i => i.InstrumentType).LoadAsync();
            await context.Entry(instrument).Reference(i => i.Area).LoadAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var instrument = await context.Instruments.FirstOrDefaultAsync(i => i.Id == id);
            if (instrument != null)
            {
                context.Instruments.Remove(instrument);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Instrument?> GetAsync(int id)
        {
            return await context.Instruments
                .Include(i => i.InstrumentType)
                .Include(i => i.Area)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Instrument>> GetAllAsync()
        {
            return await context.Instruments
                .Include(i => i.InstrumentType)
                .Include(i => i.Area)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Instrument instrument)
        {
            var existing = await context.Instruments.FindAsync(instrument.Id);
            if (existing != null)
            {
                existing.SetCode(instrument.Code);
                existing.SetName(instrument.Name);
                existing.SetSerialNumber(instrument.SerialNumber);
                existing.SetBrand(instrument.Brand);
                existing.SetModel(instrument.Model);
                existing.SetStatus(instrument.Status);
                existing.SetMaxAllowedError(instrument.MaxAllowedError);
                existing.SetCalibrationFrequencyMonths(instrument.CalibrationFrequencyMonths);
                existing.SetLastCalibrationDate(instrument.LastCalibrationDate);
                existing.SetNextCalibrationDate(instrument.NextCalibrationDate);
                existing.SetIsActive(instrument.IsActive);
                existing.SetCreatedAt(instrument.CreatedAt);
                existing.SetUpdatedAt(instrument.UpdatedAt);
                existing.SetInstrumentTypeId(instrument.InstrumentTypeId);
                existing.SetAreaId(instrument.AreaId);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}