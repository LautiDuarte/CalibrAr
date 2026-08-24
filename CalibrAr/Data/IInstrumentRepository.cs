using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface IInstrumentRepository
    {
        Task AddAsync(Instrument instrument);
        Task<bool> DeleteAsync(int id);
        Task<Instrument?> GetAsync(int id);
        Task<IEnumerable<Instrument>> GetAllAsync();
        Task<bool> UpdateAsync(Instrument instrument);
    }
}