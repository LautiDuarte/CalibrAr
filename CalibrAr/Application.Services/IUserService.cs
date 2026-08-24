using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Application.Services
{
    public interface IUserService
    {
        Task<UserDTO> AddAsync(UserDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<UserDTO?> GetAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<bool> UpdateAsync(UserDTO dto);
    }
}