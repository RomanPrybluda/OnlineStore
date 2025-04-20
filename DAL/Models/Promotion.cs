namespace DAL
{
    public class Promotion
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string ImageBannerName { get; set; } = string.Empty;

        public List<Guid> CategoryIds { get; set; } = new List<Guid>();

        public List<Guid> ProductIds { get; set; } = new List<Guid>();

    }
}
