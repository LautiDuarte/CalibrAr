using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class User
    {
        public int Id { get; private set; }
        public string FullName { get; private set; } = String.Empty;
        public string Email { get; private set; } = String.Empty;
        public string PasswordHash { get; private set; } = String.Empty;
        public UserRole Role { get; private set; }
        public DateTime? LastLoginAt { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Salt { get; private set; } = String.Empty;

        public int? PermissionGroupId { get; private set; }
        public virtual PermissionGroup? Group { get; private set; }


        public User(int id, string fullName, string email, string passwordHash, UserRole role, bool isActive, DateTime createdAt)
        {
            SetId(id);
            SetFullName(fullName);
            SetEmail(email);
            SetPasswordHash(passwordHash);
            SetRole(role);
            SetLastLoginAt(null);
            SetIsActive(isActive);
            SetCreatedAt(createdAt);
        }

        private User() { }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("Id must be greater than 0.", nameof(id));
            Id = id;
        }
        public void SetFullName(string fullname)
        {
            if (string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentException("The name cannot be null or empty.", nameof(fullname));
            FullName = fullname;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("The email cannot be null or empty.", nameof(email));
            if (!email.Contains('@'))
                throw new ArgumentException("The email format is not valid.", nameof(email));
            Email = email;
        }

        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("The password hash cannot be null or empty.", nameof(passwordHash));
            Salt = GenerateSalt();
            PasswordHash = HashPassword(passwordHash, Salt);
        }

        public void SetRole(UserRole role)
        {
            if (!Enum.IsDefined(typeof(UserRole), role))
                throw new ArgumentException("The role is not valid.", nameof(role));
            Role = role;
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            string hashedInput = HashPassword(password, Salt);
            return PasswordHash == hashedInput;
        }

        public void SetCreatedAt(DateTime createdAt)
        {
            if (createdAt == default)
                throw new ArgumentException("The creation date cannot be null.", nameof(createdAt));
            CreatedAt = createdAt;
        }

        public void SetLastLoginAt(DateTime? lastLoginAt)
        {
            LastLoginAt = lastLoginAt;
        }

        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            RandomNumberGenerator.Fill(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(32);
            return Convert.ToBase64String(hashBytes);
        }

        // Metodos para el manejo de permisos y grupos de permisos
        public void SetGroup(PermissionGroup? group)
        {
            Group = group;
            PermissionGroupId = group?.Id;
        }

        public bool HasPermission(string permissionName)
        {
            if (!IsActive || Group == null || !Group.IsActive)
                return false;
            return Group.HasPermission(permissionName);
        }

        public IEnumerable<string> GetPermissions()
        {
            if (Group == null || !Group.IsActive)
                return new List<string>();
            return Group.GetPermissionNames();
        }

        public string? GetGroupName()
        {
            return Group?.Name;
        }
    }
}