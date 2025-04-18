using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ReviewService
    {
        private readonly OnlineStoreDbContext _context;

        public ReviewService(OnlineStoreDbContext context)
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
            var product = await _context.Products
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == request.ProductId);

            if (product == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No product found with ID {request.ProductId}");

            var review = CreateReviewDTO.ToReview(request);
            _context.Reviews.Add(review);

            product.Reviews.Add(review);
            RecalculateRating(product);

            await _context.SaveChangesAsync();

            return ReviewDTO.FromReview(review);
        }

        public async Task<ReviewDTO> UpdateReviewAsync(Guid id, UpdateReviewDTO request)
        {
            var review = await _context.Reviews
                .Include(r => r.Product)
                    .ThenInclude(p => p.Reviews)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Review with ID {id} not found.");

            request.UpdateReview(review);
            _context.Reviews.Update(review);

            RecalculateRating(review.Product);

            await _context.SaveChangesAsync();

            return ReviewDTO.FromReview(review);
        }

        public async Task DeleteReviewAsync(Guid id)
        {
            var review = await _context.Reviews
                .Include(r => r.Product)
                    .ThenInclude(p => p.Reviews)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Review with ID {id} not found.");

            var product = review.Product;

            _context.Reviews.Remove(review);
            product.Reviews.Remove(review);

            RecalculateRating(product);

            await _context.SaveChangesAsync();
        }

        private void RecalculateRating(Product product)
        {
            if (product.Reviews == null || !product.Reviews.Any())
            {
                product.TotalVotes = 0;
                product.Rating = 0;
                return;
            }

            product.TotalVotes = product.Reviews.Count;
            double average = product.Reviews.Average(r => r.ReviewRating);

            product.Rating = Math.Round(average * Math.Log(1 + product.TotalVotes), 3);
        }
    }
}
