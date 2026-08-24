using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ProcedureRepository : IProcedureRepository
    {
        private readonly CalibrArContext context;

        public ProcedureRepository(CalibrArContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Procedure procedure)
        {
            context.Procedures.Add(procedure);
            await context.SaveChangesAsync();
            await context.Entry(procedure).Reference(p => p.InstrumentType).LoadAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var procedure = await context.Procedures.FirstOrDefaultAsync(p => p.Id == id);
            if (procedure != null)
            {
                context.Procedures.Remove(procedure);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Procedure?> GetAsync(int id)
        {
            return await context.Procedures.Include(p => p.InstrumentType).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Procedure>> GetAllAsync()
        {
            return await context.Procedures.Include(p => p.InstrumentType).OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Procedure procedure)
        {
            var existing = await context.Procedures.FindAsync(procedure.Id);
            if (existing != null)
            {
                existing.SetCode(procedure.Code);
                existing.SetName(procedure.Name);
                existing.SetVersionNumber(procedure.VersionNumber);
                existing.SetApprovedAt(procedure.ApprovedAt);
                existing.SetIsActive(procedure.IsActive);
                existing.SetCreatedAt(procedure.CreatedAt);
                existing.SetInstrumentTypeId(procedure.InstrumentTypeId);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}