using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface IProcedureRepository
    {
        Task AddAsync(Procedure procedure);
        Task<bool> DeleteAsync(int id);
        Task<Procedure?> GetAsync(int id);
        Task<IEnumerable<Procedure>> GetAllAsync();
        Task<bool> UpdateAsync(Procedure procedure);
    }
}