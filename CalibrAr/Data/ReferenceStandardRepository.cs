using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ReferenceStandardRepository : IReferenceStandardRepository
    {
        private readonly CalibrArContext context;

        public ReferenceStandardRepository(CalibrArContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(ReferenceStandard referenceStandard)
        {
            context.ReferenceStandards.Add(referenceStandard);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var referenceStandard = await context.ReferenceStandards.FirstOrDefaultAsync(r => r.Id == id);
            if (referenceStandard != null)
            {
                context.ReferenceStandards.Remove(referenceStandard);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ReferenceStandard?> GetAsync(int id)
        {
            return await context.ReferenceStandards.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<ReferenceStandard>> GetAllAsync()
        {
            return await context.ReferenceStandards.OrderBy(r => r.Description).ToListAsync();
        }

        public async Task<bool> UpdateAsync(ReferenceStandard referenceStandard)
        {
            var existing = await context.ReferenceStandards.FindAsync(referenceStandard.Id);
            if (existing != null)
            {
                existing.SetDescription(referenceStandard.Description);
                existing.SetCertifyingBody(referenceStandard.CertifyingBody);
                existing.SetCertificateNumber(referenceStandard.CertificateNumber);
                existing.SetCertificateIssuedAt(referenceStandard.CertificateIssuedAt);
                existing.SetCertificateExpiresAt(referenceStandard.CertificateExpiresAt);
                existing.SetIsActive(referenceStandard.IsActive);
                existing.SetCreatedAt(referenceStandard.CreatedAt);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}