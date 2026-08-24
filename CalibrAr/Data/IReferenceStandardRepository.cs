using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface IReferenceStandardRepository
    {
        Task AddAsync(ReferenceStandard referenceStandard);
        Task<bool> DeleteAsync(int id);
        Task<ReferenceStandard?> GetAsync(int id);
        Task<IEnumerable<ReferenceStandard>> GetAllAsync();
        Task<bool> UpdateAsync(ReferenceStandard referenceStandard);
    }
}