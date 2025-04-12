using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ReviewService
    {
        private readonly CraftSweetsDbContext _context;

        public ReviewService(CraftSweetsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsByProductIdAsync(Guid productId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            if (reviews == null || !reviews.Any())
                throw new CustomException(CustomExceptionType.NotFound, $"No reviews found for product ID {productId}");

            return reviews.Select(ReviewDTO.FromReview);
        }

        public async Task<ReviewDTO> GetReviewByIdAsync(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No review found with ID {id}");

            return ReviewDTO.FromReview(review);
        }

        public async Task<ReviewDTO> CreateReviewAsync(CreateReviewDTO request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No product found with ID {request.ProductId}");

            var review = CreateReviewDTO.ToReview(request);

            _context.Reviews.Add(review);

            product.TotalVotes++;
            product.Rating = (product.Reviews.Sum(r => r.ReviewRating) + review.ReviewRating) / (double)product.TotalVotes;

            await _context.SaveChangesAsync();

            return ReviewDTO.FromReview(review);
        }

        public async Task<ReviewDTO> UpdateReviewAsync(Guid id, UpdateReviewDTO request)
        {
            var review = await _context.Reviews
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Review with ID {id} not found.");

            var product = review.Product;
            if (product == null)
                throw new CustomException(CustomExceptionType.NotFound, "Product associated with this review not found.");

            product.Rating = (product.Rating * product.TotalVotes - review.ReviewRating + request.ReviewRating) / (double)product.TotalVotes;

            request.UpdateReview(review);
            _context.Reviews.Update(review);

            await _context.SaveChangesAsync();

            return ReviewDTO.FromReview(review);
        }

        public async Task DeleteReviewAsync(Guid id)
        {
            var review = await _context.Reviews
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Review with ID {id} not found.");

            var product = review.Product;
            if (product == null)
                throw new CustomException(CustomExceptionType.NotFound, "Product associated with this review not found.");

            if (product.TotalVotes > 1)
            {
                product.Rating = (product.Rating * product.TotalVotes - review.ReviewRating) / (double)(product.TotalVotes - 1);
                product.TotalVotes--;
            }
            else
            {
                product.Rating = 0;
                product.TotalVotes = 0;
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}
