using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Permission
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation properties
        public virtual ICollection<PermissionGroup> Groups { get; private set; } = new List<PermissionGroup>();

        public Permission(int id, string nombre, string descripcion, string categoria, bool activo = true)
        {
            SetId(id);
            SetName(nombre);
            SetDescription(descripcion);
            SetCategory(categoria);
            SetIsActive(activo);
        }

        // Constructor privado para Entity Framework
        private Permission() { }

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

        public void SetCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("The category cannot be null or empty.", nameof(category));

            if (category.Length > 30)
                throw new ArgumentException("The category cannot exceed 30 characters.", nameof(category));

            Category = category;
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public override string ToString()
        {
            return $"{Category}.{Name}";
        }
    }
}
