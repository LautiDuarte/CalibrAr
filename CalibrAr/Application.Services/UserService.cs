using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using DTOs;
using Domain.Model;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<UserDTO> AddAsync(UserDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Password is required to create a user.", nameof(dto.Password));

            var createdAt = DateTime.Now;
            var isActive = true;
            var role = ParseRole(dto.Role);
            User user = new User(0, dto.FullName, dto.Email, dto.Password, role, isActive, createdAt);

            await userRepository.AddAsync(user);

            dto.Id = user.Id;
            dto.CreatedAt = user.CreatedAt;
            dto.LastLoginAt = user.LastLoginAt;
            dto.Password = null;
            dto.IsActive = isActive;
            dto.GroupName = user.GetGroupName();

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await userRepository.DeleteAsync(id);
        }

        public async Task<UserDTO?> GetAsync(int id)
        {
            User? user = await userRepository.GetAsync(id);

            if (user == null)
                return null;

            return MapToDto(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(UserDTO dto)
        {
            var existing = await userRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            var role = ParseRole(dto.Role);
            // El cambio de contraseña queda fuera de esta entrega. El constructor exige un valor no vacío
            // para este parámetro, pero UserRepository.UpdateAsync nunca lee PasswordHash/Salt de "user" —
            // solo copia FullName/Email/Role/IsActive sobre la entidad trackeada — así que este valor se descarta.
            User user = new User(dto.Id, dto.FullName, dto.Email, "unused", role, dto.IsActive, existing.CreatedAt);
            return await userRepository.UpdateAsync(user);
        }

        private static UserRole ParseRole(string role)
        {
            if (!Enum.TryParse<UserRole>(role, out var parsed))
                throw new ArgumentException($"The role '{role}' is not valid.", nameof(role));
            return parsed;
        }

        private static UserDTO MapToDto(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Password = null,
                Role = user.Role.ToString(),
                LastLoginAt = user.LastLoginAt,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                GroupName = user.GetGroupName()
            };
        }
    }
}