using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<User?> GetAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<bool> UpdateAsync(User user);
    }
}