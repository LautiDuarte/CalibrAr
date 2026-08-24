using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface IInstrumentStatusHistoryRepository
    {
        Task AddAsync(InstrumentStatusHistory instrumentStatusHistory);
        Task<bool> DeleteAsync(int id);
        Task<InstrumentStatusHistory?> GetAsync(int id);
        Task<IEnumerable<InstrumentStatusHistory>> GetAllAsync();
        Task<bool> UpdateAsync(InstrumentStatusHistory instrumentStatusHistory);
    }
}