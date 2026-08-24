using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface INonConformityRepository
    {
        Task AddAsync(NonConformity nonConformity);
        Task<bool> DeleteAsync(int id);
        Task<NonConformity?> GetAsync(int id);
        Task<IEnumerable<NonConformity>> GetAllAsync();
        Task<bool> UpdateAsync(NonConformity nonConformity);
    }
}