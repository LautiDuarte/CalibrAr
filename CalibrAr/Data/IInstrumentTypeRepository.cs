using Domain.Model;

namespace Data
{
    public interface IInstrumentTypeRepository
    {
        Task AddAsync(InstrumentType instrumentType);
        Task<bool> DeleteAsync(int id);
        Task<InstrumentType?> GetAsync(int id);
        Task<IEnumerable<InstrumentType>> GetAllAsync();
        Task<bool> UpdateAsync(InstrumentType instrumentType);
    }
}

