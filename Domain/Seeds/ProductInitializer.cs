using DAL;

namespace Domain
{
    public class ProductInitializer
    {
        private readonly OnlineStoreDbContext _context;
        private static readonly Random _random = new Random();

        private readonly int needsProductsQuantity = 100;

        public ProductInitializer(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public void InitializeProducts()
        {
            var existingProductCount = _context.Products.Count();

            if (existingProductCount < needsProductsQuantity)
            {
                for (int i = 0; i < (needsProductsQuantity - existingProductCount); i++)
                {

                    var category = _context.Categories.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                    if (category == null) continue;

                    var productName = $"Product {i + 1}";
                    var productDescription = $"Description for Product {i + 1}";
                    var productPrice = (decimal)(_random.NextDouble() * (1000 - 10) + 10);
                    var imageUrl = $"https://example.com/images/product{i + 1}.jpg";

                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = productName,
                        Description = productDescription,
                        Price = productPrice,
                        ImageUrl = imageUrl,
                        CategoryId = category.Id,
                        Category = category,
                    };

                    var productDetails = new ProductDetails
                    {
                        ProductId = product.Id,
                        Sku = $"SKU-{Guid.NewGuid()}",
                        Attributes = $"Attributes for Product {i + 1}",
                        StockQuantity = _random.Next(1, 100)
                    };

                    _context.Products.Add(product);
                    _context.ProductDetails.Add(productDetails);
                }

                _context.SaveChanges();
            }
        }
    }
}
