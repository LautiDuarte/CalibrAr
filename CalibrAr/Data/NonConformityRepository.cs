using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class NonConformityRepository : INonConformityRepository
    {
        private readonly CalibrArContext context;

        public NonConformityRepository(CalibrArContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(NonConformity nonConformity)
        {
            context.NonConformities.Add(nonConformity);
            await context.SaveChangesAsync();
            await context.Entry(nonConformity).Reference(n => n.Instrument).LoadAsync();
            await context.Entry(nonConformity).Reference(n => n.Calibration).LoadAsync();
            await context.Entry(nonConformity).Reference(n => n.DetectedByUser).LoadAsync();
            await context.Entry(nonConformity).Reference(n => n.ClosedByUser).LoadAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var nonConformity = await context.NonConformities.FirstOrDefaultAsync(n => n.Id == id);
            if (nonConformity != null)
            {
                context.NonConformities.Remove(nonConformity);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<NonConformity?> GetAsync(int id)
        {
            return await context.NonConformities
                .Include(n => n.Instrument)
                .Include(n => n.Calibration)
                .Include(n => n.DetectedByUser)
                .Include(n => n.ClosedByUser)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<NonConformity>> GetAllAsync()
        {
            return await context.NonConformities
                .Include(n => n.Instrument)
                .Include(n => n.Calibration)
                .Include(n => n.DetectedByUser)
                .Include(n => n.ClosedByUser)
                .OrderByDescending(n => n.OpenedAt)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(NonConformity nonConformity)
        {
            var existing = await context.NonConformities.FindAsync(nonConformity.Id);
            if (existing != null)
            {
                existing.SetCode(nonConformity.Code);
                existing.SetDescription(nonConformity.Description);
                existing.SetOrigin(nonConformity.Origin);
                existing.SetStatus(nonConformity.Status);
                existing.SetCorrectiveAction(nonConformity.CorrectiveAction);
                existing.SetOpenedAt(nonConformity.OpenedAt);
                existing.SetClosedAt(nonConformity.ClosedAt);
                existing.SetCreatedAt(nonConformity.CreatedAt);
                existing.SetInstrumentId(nonConformity.InstrumentId);
                existing.SetCalibrationId(nonConformity.CalibrationId);
                existing.SetDetectedByUserId(nonConformity.DetectedByUserId);
                existing.SetClosedByUserId(nonConformity.ClosedByUserId);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}