using DAL;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class CreateReviewDTO
    {
        [Required]
        public Guid ProductId { get; set; }

        public Guid UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int ReviewRating { get; set; }

        [StringLength(1000, ErrorMessage = "Comment must not exceed 1000 characters.")]
        public string? Comment { get; set; }

        public static Review ToReview(CreateReviewDTO request)
        {
            return new Review
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                UserId = request.UserId.ToString(),
                ReviewRating = request.ReviewRating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
