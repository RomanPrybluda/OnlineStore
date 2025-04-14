namespace DAL
{
    public class Review
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public string UserId { get; set; }

        public AppUser AppUser { get; set; } = null!;

        public int ReviewRating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
