namespace Domain.Model
{
    public class Location
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? Address { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }


        public Location(int id, string name, string? address, bool isActive, DateTime createdAt)
        {
            SetId(id);
            SetName(name);
            SetAddress(address);
            SetIsActive(isActive);
            SetCreatedAt(createdAt);

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

        public void SetAddress(string? address)
        {
            Address = address;
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
    }
}
