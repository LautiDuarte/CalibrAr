using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class InstrumentStatusHistoryRepository : IInstrumentStatusHistoryRepository
    {
        private readonly CalibrArContext context;

        public InstrumentStatusHistoryRepository(CalibrArContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(InstrumentStatusHistory instrumentStatusHistory)
        {
            context.InstrumentStatusHistories.Add(instrumentStatusHistory);
            await context.SaveChangesAsync();
            await context.Entry(instrumentStatusHistory).Reference(i => i.Instrument).LoadAsync();
            await context.Entry(instrumentStatusHistory).Reference(i => i.ChangedByUser).LoadAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var instrumentStatusHistory = await context.InstrumentStatusHistories.FirstOrDefaultAsync(i => i.Id == id);
            if (instrumentStatusHistory != null)
            {
                context.InstrumentStatusHistories.Remove(instrumentStatusHistory);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<InstrumentStatusHistory?> GetAsync(int id)
        {
            return await context.InstrumentStatusHistories
                .Include(i => i.Instrument)
                .Include(i => i.ChangedByUser)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<InstrumentStatusHistory>> GetAllAsync()
        {
            return await context.InstrumentStatusHistories
                .Include(i => i.Instrument)
                .Include(i => i.ChangedByUser)
                .OrderByDescending(i => i.ChangedAt)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(InstrumentStatusHistory instrumentStatusHistory)
        {
            var existing = await context.InstrumentStatusHistories.FindAsync(instrumentStatusHistory.Id);
            if (existing != null)
            {
                existing.SetPreviousStatus(instrumentStatusHistory.PreviousStatus);
                existing.SetNewStatus(instrumentStatusHistory.NewStatus);
                existing.SetReason(instrumentStatusHistory.Reason);
                existing.SetChangedAt(instrumentStatusHistory.ChangedAt);
                existing.SetInstrumentId(instrumentStatusHistory.InstrumentId);
                existing.SetChangedByUserId(instrumentStatusHistory.ChangedByUserId);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}