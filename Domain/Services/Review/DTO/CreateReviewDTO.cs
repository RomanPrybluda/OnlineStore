using DAL;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class CreateReviewDTO
    {
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// User ID in UUID format (example: "ef654a7e-84ca-4b0c-93de-4abc7d3cbce8")
        /// </summary>
        [Required]
        [RegularExpression(@"^[{(]?[0-9A-Fa-f]{8}(-[0-9A-Fa-f]{4}){3}-[0-9A-Fa-f]{12}[)}]?$",
            ErrorMessage = "UserId must be in valid UUID format")]
        public string UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int ReviewRating { get; set; }

        public string? Comment { get; set; }

        public static Review ToReview(CreateReviewDTO request)
        {
            return new Review
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                UserId = request.UserId,
                ReviewRating = request.ReviewRating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
