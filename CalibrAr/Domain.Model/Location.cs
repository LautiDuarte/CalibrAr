namespace Domain.Model
{
    public class Location
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
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
                throw new ArgumentException("Id must be greater than 0.", nameof(id));
            Id = id;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The name cannot be null or empty.", nameof(name));
            Name = name;
        }

        public void SetAddress(string address)
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
                throw new ArgumentException("The creation date cannot be null.", nameof(createdAt));
            CreatedAt = createdAt;
        }
    }
}
