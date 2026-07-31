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

        public void SetCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("La categoría no puede ser nula o vacía.", nameof(category));

            if (category.Length > 30)
                throw new ArgumentException("La categoría no puede exceder 30 caracteres.", nameof(category));

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
