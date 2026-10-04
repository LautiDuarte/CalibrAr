using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CalibrationRepository : ICalibrationRepository
    {
        private readonly CalibrArContext context;

        public CalibrationRepository(CalibrArContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Calibration calibration)
        {
            context.Calibrations.Add(calibration);
            await context.SaveChangesAsync();
            await context.Entry(calibration).Reference(c => c.Instrument).LoadAsync();
            await context.Entry(calibration).Reference(c => c.Procedure).LoadAsync();
            await context.Entry(calibration).Reference(c => c.PerformedByUser).LoadAsync();
            await context.Entry(calibration).Reference(c => c.ApprovedByUser).LoadAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var calibration = await context.Calibrations.FirstOrDefaultAsync(c => c.Id == id);
            if (calibration != null)
            {
                context.Calibrations.Remove(calibration);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Calibration?> GetAsync(int id)
        {
            return await context.Calibrations
                .Include(c => c.Measurements)
                .Include(c => c.Instrument)
                .Include(c => c.Procedure)
                .Include(c => c.PerformedByUser)
                .Include(c => c.ApprovedByUser)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Calibration>> GetAllAsync()
        {
            return await context.Calibrations
                .Include(c => c.Measurements)
                .Include(c => c.Instrument)
                .Include(c => c.Procedure)
                .Include(c => c.PerformedByUser)
                .Include(c => c.ApprovedByUser)
                .OrderByDescending(c => c.CalibrationDate)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Calibration calibration)
        {
            var existing = await context.Calibrations
                .Include(c => c.Measurements)
                .FirstOrDefaultAsync(c => c.Id == calibration.Id);
            if (existing != null)
            {
                existing.SetCalibrationDate(calibration.CalibrationDate);
                existing.SetInterventionType(calibration.InterventionType);
                existing.SetIsExternal(calibration.IsExternal);
                existing.SetExternalLab(calibration.ExternalLab);
                existing.SetCertificateNumber(calibration.CertificateNumber);
                existing.SetResult(calibration.Result);
                existing.SetRestrictionDetail(calibration.RestrictionDetail);
                existing.SetNotes(calibration.Notes);
                existing.SetInstrumentId(calibration.InstrumentId);
                existing.SetProcedureId(calibration.ProcedureId);
                existing.SetPerformedByUserId(calibration.PerformedByUserId);
                existing.SetApprovedByUserId(calibration.ApprovedByUserId);

                SyncMeasurements(existing, calibration.Measurements);

                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Calibration?> GetLatestByInstrumentAsync(int instrumentId)
        {
            return await context.Calibrations
                .AsNoTracking()
                .Where(c => c.InstrumentId == instrumentId)
                .OrderByDescending(c => c.CalibrationDate)
                .ThenByDescending(c => c.Id)
                .FirstOrDefaultAsync();
        }

        private void SyncMeasurements(Calibration existing, ICollection<CalibrationMeasurement> incoming)
        {
            var incomingIds = incoming.Where(m => m.Id != 0).Select(m => m.Id).ToHashSet();

            // Borrar los que ya no están
            foreach (var removed in existing.Measurements.Where(m => !incomingIds.Contains(m.Id)).ToList())
                context.CalibrationMeasurements.Remove(removed);

            foreach (var m in incoming)
            {
                if (m.Id == 0)
                {
                    existing.Measurements.Add(new CalibrationMeasurement(
                        0, m.NominalValue, m.MeasuredValue, m.Error, m.IsWithinTolerance, m.Notes, existing.Id));
                    continue;
                }

                var tracked = existing.Measurements.FirstOrDefault(x => x.Id == m.Id)
                    ?? throw new KeyNotFoundException($"Measurement {m.Id} does not belong to calibration {existing.Id}.");

                tracked.SetNominalValue(m.NominalValue);
                tracked.SetMeasuredValue(m.MeasuredValue);
                tracked.SetError(m.Error);
                tracked.SetIsWithinTolerance(m.IsWithinTolerance);
                tracked.SetNotes(m.Notes);
            }
        }
    }
}