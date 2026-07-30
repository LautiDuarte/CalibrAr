using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Model
{
    public class User
    {
        public int Id { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRole Role { get; private set; }
        public DateTime? LastLoginAt { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Salt { get; private set; }


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

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor que 0.", nameof(id));
            Id = id;
        }
        public void SetFullName(string fullname)
        {
            if (string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(fullname));
            FullName = fullname;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede ser nulo o vacío.", nameof(email));
            if (!email.Contains('@'))
                throw new ArgumentException("El email no tiene un formato válido.", nameof(email));
            Email = email;
        }

        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("El hash de contraseña no puede ser nulo o vacío.", nameof(passwordHash));
            Salt = GenerateSalt();
            PasswordHash = HashPassword(passwordHash, Salt);
        }

        public void SetRole(UserRole role)
        {
            if (!Enum.IsDefined(typeof(UserRole), role))
                throw new ArgumentException("El rol especificado no es válido.", nameof(role));
            Role = role;
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void SetCreatedAt(DateTime createdAt)
        {
            if (createdAt == default)
                throw new ArgumentException("La fecha de alta no puede ser nula.", nameof(createdAt));
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
    }
}
