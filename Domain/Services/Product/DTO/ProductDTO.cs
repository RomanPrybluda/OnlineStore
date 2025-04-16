using DAL;

namespace Domain
{
    public class ProductDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string MainImageName { get; set; } = string.Empty;

        public string SortDescription { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public static ProductDTO FromProduct(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                SortDescription = product.SortDescription,
                IsActive = product.IsActive,
                Price = product.Price,
                MainImageName = product.MainImageBaseName
            };
        }
    }
}
