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
                var categories = _context.Categories.ToList();
                if (!categories.Any())
                {
                    Console.WriteLine("No categories found in the database.");
                    return;
                }

                for (int i = 0; i < (needsProductsQuantity - existingProductCount); i++)
                {
                    var category = _context.Categories.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                    if (category == null) continue;

                    var productName = $"Product {i + 1}";
                    var productDescription = $"Description for Product {i + 1}";
                    var productPrice = (decimal)(_random.NextDouble() * (1000 - 10) + 10);
                    var imageUrl = $"https://example.com/images/product{i + 1}.jpg";
                    var sku = $"SKU-{Guid.NewGuid()}";
                    var stockQuantity = _random.Next(1, 100);

                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = productName,
                        Description = productDescription,
                        Price = productPrice,
                        ImageUrl = imageUrl,
                        Sku = sku,
                        StockQuantity = stockQuantity,
                        IsActive = true,
                        CategoryId = category.Id,
                        Category = category,
                        Rating = Math.Round(_random.NextDouble() * 5, 1),
                        TotalVotes = _random.Next(0, 500)
                    };

                    _context.Products.Add(product);
                }

                _context.SaveChanges();
            }
        }
    }
}
