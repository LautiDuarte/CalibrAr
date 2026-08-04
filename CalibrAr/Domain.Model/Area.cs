using System;
namespace Domain.Model
{
    public class Area
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? Responsible { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private int _locationId;
        private Location? _location;

        public int LocationId
        {
            get => _location?.Id ?? _locationId;
            private set => _locationId = value;
        }

        public Location? Location
        {
            get => _location;
            private set
            {
                _location = value;
                _locationId = value?.Id ?? _locationId;
            }
        }

        public Area(int id, string name, string? responsible, bool isActive, DateTime createdAt, int locationId)
        {
            SetId(id);
            SetName(name);
            SetResponsible(responsible);
            SetIsActive(isActive);
            SetCreatedAt(createdAt);
            SetLocationId(locationId);
        }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor que 0.", nameof(id));
            Id = id;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(name));
            Name = name;
        }

        public void SetResponsible(string? responsible)
        {
            Responsible = responsible;
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

        public void SetLocationId(int locationId)
        {
            if (locationId <= 0)
                throw new ArgumentException("El LocationId debe ser mayor que 0.", nameof(locationId));

            // Si el objeto cargado ya no corresponde al nuevo id, se invalida
            if (_location != null && _location.Id != locationId)
                Location = null; // pasa por el setter de la propiedad, coherente en un solo lugar

            LocationId = locationId; // usa el setter de la propiedad, no el campo directo
        }

        public void SetLocation(Location location)
        {
            ArgumentNullException.ThrowIfNull(location);
            Location = location; // el setter de la propiedad ya sincroniza _locationId
        }
    }
}