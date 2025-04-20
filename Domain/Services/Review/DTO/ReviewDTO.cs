using DAL;

namespace Domain
{
    public class ReviewDTO
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string UserId { get; set; }

        public int ReviewRating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public static ReviewDTO FromReview(Review review) => new()
        {
            Id = review.Id,
            ProductId = review.ProductId,
            UserId = review.UserId.ToString(),
            ReviewRating = review.ReviewRating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        };
    }
}
