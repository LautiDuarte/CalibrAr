namespace Domain.Model
{
    public class Location
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = String.Empty;
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
                throw new ArgumentException("El id debe ser mayor que 0.");
            Id = id;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede estar vacío.");
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
                throw new ArgumentException("La fecha de creación no puede estar vacía.");
            CreatedAt = createdAt;
        }
    }
}
