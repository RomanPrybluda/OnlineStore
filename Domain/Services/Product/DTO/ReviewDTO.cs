using DAL;

namespace Domain
{
    public class ReviewDTO
    {
        public Guid Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public double Rating { get; set; }

        public static ReviewDTO FromReview(Review review)
        {
            return new ReviewDTO
            {
                Id = review.Id,
                Comment = review.Comment,
                Rating = review.Rating
            };
        }
    }

}
