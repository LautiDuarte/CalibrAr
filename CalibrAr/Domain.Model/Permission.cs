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
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Category { get; private set; } = string.Empty;
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
                throw new ArgumentException("El id debe ser mayor que 0.");
            Id = id;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede estar vacío.");

            if (name.Length > 50)
                throw new ArgumentException("El nombre no puede superar los 50 caracteres.");

            Name = name;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción no puede estar vacía.");

            if (description.Length > 200)
                throw new ArgumentException("La descripción no puede superar los 200 caracteres.");
            Description = description;
        }

        public void SetCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("La categoría no puede estar vacía.");

            if (category.Length > 30)
                throw new ArgumentException("La categoría no puede superar los 30 caracteres.");

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
