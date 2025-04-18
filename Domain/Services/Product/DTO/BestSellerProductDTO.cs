using DAL;

namespace Domain
{
    public class BestSellerProductDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? MainImageBaseName { get; set; }

        public int SoldQuantity { get; set; }

        public static BestSellerProductDTO FromProduct(Product product)
        {
            return new BestSellerProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                MainImageBaseName = product.MainImageBaseName,
                SoldQuantity = product.SoldQuantity
            };
        }
    }
}
