using DAL;

namespace Domain
{
    public class CreateProductDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string Sku { get; set; } = string.Empty;

        public int StockQuantity { get; set; }

        public Guid CategoryId { get; set; }

        public static Product ToProduct(CreateProductDTO request)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                Sku = request.Sku,
                StockQuantity = request.StockQuantity,
                IsActive = true,
                CategoryId = request.CategoryId,
                Rating = 0,
                TotalVotes = 0
            };
        }
    }
}
