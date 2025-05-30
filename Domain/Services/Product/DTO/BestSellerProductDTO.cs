using DAL;

namespace Domain
{
    public class BestSellerProductDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? MainImageBaseName { get; set; }

        public string SortDescription { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int SoldQuantity { get; set; }

        public double Rating { get; set; }

        public static BestSellerProductDTO FromProduct(Product product)
        {
            return new BestSellerProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                SortDescription = product.SortDescription,
                Price = product.Price,
                MainImageBaseName = product.MainImageBaseName,
                SoldQuantity = product.SoldQuantity,
                Rating = product.Rating
            };
        }
    }
}
