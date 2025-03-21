using DAL;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class UpdateReviewDTO
    {
        [Required]
        [Range(1, 5)]
        public int ReviewRating { get; set; }

        public string? Comment { get; set; }

        public void UpdateReview(Review request)
        {
            request.ReviewRating = request.ReviewRating;
            request.Comment = Comment ?? request.Comment;
        }
    }
}
