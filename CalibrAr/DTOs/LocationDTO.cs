namespace DTOs
{
    public class LocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
