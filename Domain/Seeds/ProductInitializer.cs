using DAL;

namespace Domain
{
    public class ProductInitializer
    {
        private readonly OnlineStoreDbContext _context;
        private static readonly Random _random = new Random();

        private readonly int needsProductsQuantity = 100;
        private static readonly string[] SweetNames = new string[]
        {
            "Chocolate Bar", "Gummy Bears", "Marshmallow", "Candy Cane", "Caramel Popcorn", "Lollipop", "Fudge", "Nougat", "Toffee", "Licorice",
            "Truffle", "Cheesecake", "Brownie", "Macaron", "Ice Cream", "Cotton Candy", "Pudding", "Doughnut", "Cupcake", "Jelly Beans",
            "Muffin", "Tiramisu", "Shortbread", "Waffle", "Churros", "Eclair", "Chocolate Chip Cookie", "Banana Bread", "Fruit Tart", "Peanut Brittle",
            "Honeycomb", "Molten Lava Cake", "Pancakes", "Strawberry Sundae", "Meringue", "Profiteroles", "Candy Floss", "Gingerbread", "Almond Brittle", "Chocolate Mousse",
            "Baklava", "Pecan Pie", "Coconut Macaroon", "Apple Pie", "Peach Cobbler", "Creme Brulee", "Rock Candy", "Turkish Delight", "Sugar Cookies", "Butter Cookies",
            "Lemon Bars", "Key Lime Pie", "Raspberry Sorbet", "Blueberry Muffin", "Cherry Pie", "Pumpkin Pie", "Red Velvet Cake", "Black Forest Cake", "Angel Food Cake", "Spiced Cake",
            "Carrot Cake", "Opera Cake", "Vanilla Ice Cream", "Hazelnut Praline", "Maple Taffy", "Sweet Potato Pie", "Walnut Brownie", "Chocolate Fudge Cake", "Chocolate Truffles", "Pistachio Ice Cream",
            "Marzipan", "Berry Tart", "Rum Cake", "Sour Candy", "Milkshake", "Chocolate Spread", "Strawberry Cheesecake", "Cinnamon Roll", "Ginger Snap", "Cherry Clafoutis",
            "Tropical Fruit Tart", "Berry Compote", "Pumpkin Roll", "Mango Sorbet", "Coconut Cake", "Matcha Ice Cream", "Peanut Butter Fudge", "Saltwater Taffy", "Mint Chocolate Chip Ice Cream", "Bubblegum",
            "Chocolate Pudding", "Tutti Frutti", "Raspberry Coulis", "Espresso Brownie", "Mocha Cake", "Danish Pastry", "Funnel Cake", "S'mores", "Cinnamon Doughnut", "Caramel Pudding"
        };

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

                    var productName = SweetNames[_random.Next(SweetNames.Length)];
                    var productDescription = $"Delicious {productName} for your sweet cravings!";
                    var sortDescription = productDescription.Length > 50 ? productDescription.Substring(0, 50) + "..." : productDescription;
                    var productPrice = (decimal)(_random.NextDouble() * (1000 - 10) + 10);
                    var imageName = $"mainImageName{i + 1}.webp";
                    var baseImageName = $"imageName{i + 1}";
                    var imageExtension = ".webp";
                    var imageNames = new List<string>
                    {
                        $"{baseImageName}-1{imageExtension}",
                        $"{baseImageName}-2{imageExtension}",
                        $"{baseImageName}-3{imageExtension}"
                    };
                    var sku = $"SKU-{Guid.NewGuid()}";
                    var stockQuantity = _random.Next(1, 100);

                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = productName,
                        Description = productDescription,
                        SortDescription = sortDescription,
                        Price = productPrice,
                        MainImageBaseName = imageName,
                        ImageBaseNames = imageNames,
                        Sku = sku,
                        StockQuantity = stockQuantity,
                        IsActive = _random.Next(0, 2) == 1,
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
