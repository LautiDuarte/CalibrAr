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
                throw new ArgumentException("Id must be greater than 0.", nameof(id));
            Id = id;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The name cannot be null or empty.", nameof(name));

            if (name.Length > 50)
                throw new ArgumentException("The name cannot exceed 50 characters.", nameof(name));

            Name = name;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("The description cannot be null or empty.", nameof(description));

            if (description.Length > 200)
                throw new ArgumentException("The description cannot exceed 200 characters.", nameof(description));

            Description = description;
        }

        public void SetCreatedAt(DateTime createdAt)
        {
            if (createdAt == default)
                throw new ArgumentException("The creation date cannot be null.", nameof(createdAt));
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
