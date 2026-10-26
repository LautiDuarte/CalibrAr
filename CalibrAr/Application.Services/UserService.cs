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

            if (dto.Password.Length < 8)
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");

            await EnsureEmailIsUniqueAsync(dto.Email);
            var role = ParseRole(dto.Role);
            var group = await ResolveGroupAsync(role);

            var createdAt = DateTime.Now;
            var isActive = true;
            User user = new User(0, dto.FullName, dto.Email, dto.Password, role, isActive, createdAt);
            user.SetGroup(group);

            await userRepository.AddAsync(user);

            dto.Id = user.Id;
            dto.CreatedAt = user.CreatedAt;
            dto.LastLoginAt = user.LastLoginAt;
            dto.Password = null;
            dto.IsActive = isActive;
            dto.GroupName = user.GetGroupName();

            return dto;
        }

        public async Task<bool> DeleteAsync(int id, int? requesterUserId = null)
        {
            var existing = await userRepository.GetAsync(id);
            if (existing == null)
                return false;

            if (requesterUserId == id)
                throw new ArgumentException("You cannot delete your own user.");

            await EnsureAnotherAdministratorRemainsAsync(existing);

            if (await userRepository.IsReferencedAsync(id))
                throw new InvalidOperationException("The user is referenced by calibrations, non conformities or status history and cannot be deleted. Deactivate it instead.");

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

            // Nadie puede cambiarse el rol ni desactivarse a sí mismo
            if (dto.RequestedByUserId == dto.Id)
            {
                if (role != existing.Role)
                    throw new ArgumentException("You cannot change your own role.", nameof(dto.Role));
                if (!dto.IsActive)
                    throw new ArgumentException("You cannot deactivate your own user.", nameof(dto.IsActive));
            }

            // Si deja de ser administrador activo, tiene que quedar otro
            if (role != UserRole.Administrador || !dto.IsActive)
                await EnsureAnotherAdministratorRemainsAsync(existing);

            await EnsureEmailIsUniqueAsync(dto.Email, dto.Id);
            var group = await ResolveGroupAsync(role);

            // El cambio de contraseña queda fuera de esta entrega: el constructor exige un valor, pero
            // UserRepository.UpdateAsync nunca copia PasswordHash/Salt, así que "unused" se descarta.
            User user = new User(dto.Id, dto.FullName, dto.Email, "unused", role, dto.IsActive, existing.CreatedAt);
            user.SetGroup(group);
            return await userRepository.UpdateAsync(user);
        }

        private async Task EnsureEmailIsUniqueAsync(string email, int? excludeId = null)
        {
            var normalized = email?.Trim() ?? string.Empty;
            if (await userRepository.ExistsByEmailAsync(normalized, excludeId))
                throw new InvalidOperationException($"There is already a user with email '{normalized}'.");
        }

        // El sistema no puede quedar sin administradores activos
        private async Task EnsureAnotherAdministratorRemainsAsync(User existing)
        {
            if (existing.Role == UserRole.Administrador && existing.IsActive
                && await userRepository.CountOtherActiveAdministratorsAsync(existing.Id) == 0)
                throw new InvalidOperationException("There must be at least one active administrator.");
        }

        // El grupo de permisos se asigna según el rol (nombres del seed de CalibrArContext)
        private async Task<PermissionGroup> ResolveGroupAsync(UserRole role)
        {
            string groupName = role switch
            {
                UserRole.Administrador => "Administrator",
                UserRole.Responsable => "Responsible",
                UserRole.Operador => "Operator",
                UserRole.Auditor => "Auditor",
                _ => throw new ArgumentException($"The role '{role}' is not valid.", nameof(role))
            };
            return await userRepository.GetGroupByNameAsync(groupName)
                ?? throw new KeyNotFoundException($"There is no permission group named '{groupName}'.");
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