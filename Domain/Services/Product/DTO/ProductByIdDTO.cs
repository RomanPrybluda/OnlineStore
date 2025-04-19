using DAL;

namespace Domain
{
    public class ProductByIdDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string MainProductImage { get; set; } = string.Empty;

        public List<string> ProductImages { get; set; } = new();

        public string Sku { get; set; } = string.Empty;

        public double Rating { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }

        public List<ReviewDTO> Reviews { get; set; } = new();

        public Guid CategoryId { get; set; }

        public static ProductByIdDTO FromProduct(Product product)
        {
            return new ProductByIdDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                MainProductImage = product.MainImageBaseName ?? string.Empty,
                ProductImages = product.ImageBaseNames,
                Sku = product.Sku,
                Rating = product.Rating,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                Reviews = product.Reviews.Select(ReviewDTO.FromReview).ToList()
            };
        }

    }
}
