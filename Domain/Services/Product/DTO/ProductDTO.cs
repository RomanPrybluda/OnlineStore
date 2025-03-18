using DAL;

namespace Domain
{
    public class ProductDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }

        public bool IsActive { get; set; }

        public static ProductDTO FromProduct(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                IsActive = product.IsActive,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId
            };
        }
    }
}
