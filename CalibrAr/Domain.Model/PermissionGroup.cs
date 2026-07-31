using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class PermissionGroup
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Navigation properties
        public virtual ICollection<Permission> Permissions { get; private set; } = new List<Permission>();

        public PermissionGroup(int id, string name, string description, DateTime createdAt, bool isActive = true)
        {
            SetId(id);
            SetName(name);
            SetDescription(description);
            SetCreatedAt(createdAt);
            SetIsActive(isActive);
        }

        // Constructor privado para Entity Framework
        private PermissionGroup() { }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual a 0.", nameof(id));
            Id = id;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(name));

            if (name.Length > 50)
                throw new ArgumentException("El nombre no puede exceder 50 caracteres.", nameof(name));

            Name = name;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción no puede ser nula o vacía.", nameof(description));

            if (description.Length > 200)
                throw new ArgumentException("La descripción no puede exceder 200 caracteres.", nameof(description));

            Description = description;
        }

        public void SetCreatedAt(DateTime createdAt)
        {
            if (createdAt == default)
                throw new ArgumentException("La fecha de creación no puede ser nula.", nameof(createdAt));
            CreatedAt = createdAt;
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void AddPermission(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            if (!Permissions.Contains(permission))
            {
                Permissions.Add(permission);
            }
        }

        public void RemovePermission(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            Permissions.Remove(permission);
        }

        public bool HasPermission(string permissionName)
        {
            return IsActive && Permissions.Any(p => p.IsActive && p.Name.Equals(permissionName, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<string> GetPermissionNames()
        {
            return Permissions
                .Where(p => p.IsActive)
                .Select(p => $"{p.Category}.{p.Name}")
                .ToList();
        }
    }
}
