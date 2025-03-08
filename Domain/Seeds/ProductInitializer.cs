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
                    var categoryId = _random.Next(1, _context.Categories.Count() + 1);
                    var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);

                    var productName = $"Product {i + 1}";
                    var productDescription = $"Description for Product {i + 1}";
                    var productPrice = (decimal)(_random.NextDouble() * (1000 - 10) + 10);
                    var imageUrl = $"https://example.com/images/product{i + 1}.jpg";

                    var product = new Product
                    {
                        Name = productName,
                        Description = productDescription,
                        Price = productPrice,
                        ImageUrl = imageUrl,
                        CategoryId = categoryId,
                        Category = category,
                    };

                    _context.Products.Add(product);
                    _context.SaveChanges();

                    var productDetails = new ProductDetails
                    {
                        ProductId = product.Id,
                        Sku = $"SKU-{Guid.NewGuid()}",
                        Attributes = $"Attributes for Product {i + 1}",
                        StockQuantity = _random.Next(1, 100)
                    };

                    _context.ProductDetails.Add(productDetails);

                    _context.SaveChanges();
                }
            }
        }
    }
}

