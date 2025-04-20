using DAL;

namespace Domain
{
    public class ProductInitializer
    {
        private readonly OnlineStoreDbContext _context;
        private static readonly Random _random = new Random();

        private readonly int needsProductsQuantity = 100;

        private static readonly Guid[] ProductGuids = new Guid[]
        {
            Guid.Parse("11111111-1111-1111-1111-111111111001"),
            Guid.Parse("11111111-1111-1111-1111-111111111002"),
            Guid.Parse("11111111-1111-1111-1111-111111111003"),
            Guid.Parse("11111111-1111-1111-1111-111111111004"),
            Guid.Parse("11111111-1111-1111-1111-111111111005"),
            Guid.Parse("11111111-1111-1111-1111-111111111006"),
            Guid.Parse("11111111-1111-1111-1111-111111111007"),
            Guid.Parse("11111111-1111-1111-1111-111111111008"),
            Guid.Parse("11111111-1111-1111-1111-111111111009"),
            Guid.Parse("11111111-1111-1111-1111-111111111010"),
            Guid.Parse("11111111-1111-1111-1111-111111111011"),
            Guid.Parse("11111111-1111-1111-1111-111111111012"),
            Guid.Parse("11111111-1111-1111-1111-111111111013"),
            Guid.Parse("11111111-1111-1111-1111-111111111014"),
            Guid.Parse("11111111-1111-1111-1111-111111111015"),
            Guid.Parse("11111111-1111-1111-1111-111111111016"),
            Guid.Parse("11111111-1111-1111-1111-111111111017"),
            Guid.Parse("11111111-1111-1111-1111-111111111018"),
            Guid.Parse("11111111-1111-1111-1111-111111111019"),
            Guid.Parse("11111111-1111-1111-1111-111111111020"),
            Guid.Parse("11111111-1111-1111-1111-111111111021"),
            Guid.Parse("11111111-1111-1111-1111-111111111022"),
            Guid.Parse("11111111-1111-1111-1111-111111111023"),
            Guid.Parse("11111111-1111-1111-1111-111111111024"),
            Guid.Parse("11111111-1111-1111-1111-111111111025"),
            Guid.Parse("11111111-1111-1111-1111-111111111026"),
            Guid.Parse("11111111-1111-1111-1111-111111111027"),
            Guid.Parse("11111111-1111-1111-1111-111111111028"),
            Guid.Parse("11111111-1111-1111-1111-111111111029"),
            Guid.Parse("11111111-1111-1111-1111-111111111030"),
            Guid.Parse("11111111-1111-1111-1111-111111111031"),
            Guid.Parse("11111111-1111-1111-1111-111111111032"),
            Guid.Parse("11111111-1111-1111-1111-111111111033"),
            Guid.Parse("11111111-1111-1111-1111-111111111034"),
            Guid.Parse("11111111-1111-1111-1111-111111111035"),
            Guid.Parse("11111111-1111-1111-1111-111111111036"),
            Guid.Parse("11111111-1111-1111-1111-111111111037"),
            Guid.Parse("11111111-1111-1111-1111-111111111038"),
            Guid.Parse("11111111-1111-1111-1111-111111111039"),
            Guid.Parse("11111111-1111-1111-1111-111111111040"),
            Guid.Parse("11111111-1111-1111-1111-111111111041"),
            Guid.Parse("11111111-1111-1111-1111-111111111042"),
            Guid.Parse("11111111-1111-1111-1111-111111111043"),
            Guid.Parse("11111111-1111-1111-1111-111111111044"),
            Guid.Parse("11111111-1111-1111-1111-111111111045"),
            Guid.Parse("11111111-1111-1111-1111-111111111046"),
            Guid.Parse("11111111-1111-1111-1111-111111111047"),
            Guid.Parse("11111111-1111-1111-1111-111111111048"),
            Guid.Parse("11111111-1111-1111-1111-111111111049"),
            Guid.Parse("11111111-1111-1111-1111-111111111050"),
            Guid.Parse("11111111-1111-1111-1111-111111111051"),
            Guid.Parse("11111111-1111-1111-1111-111111111052"),
            Guid.Parse("11111111-1111-1111-1111-111111111053"),
            Guid.Parse("11111111-1111-1111-1111-111111111054"),
            Guid.Parse("11111111-1111-1111-1111-111111111055"),
            Guid.Parse("11111111-1111-1111-1111-111111111056"),
            Guid.Parse("11111111-1111-1111-1111-111111111057"),
            Guid.Parse("11111111-1111-1111-1111-111111111058"),
            Guid.Parse("11111111-1111-1111-1111-111111111059"),
            Guid.Parse("11111111-1111-1111-1111-111111111060"),
            Guid.Parse("11111111-1111-1111-1111-111111111061"),
            Guid.Parse("11111111-1111-1111-1111-111111111062"),
            Guid.Parse("11111111-1111-1111-1111-111111111063"),
            Guid.Parse("11111111-1111-1111-1111-111111111064"),
            Guid.Parse("11111111-1111-1111-1111-111111111065"),
            Guid.Parse("11111111-1111-1111-1111-111111111066"),
            Guid.Parse("11111111-1111-1111-1111-111111111067"),
            Guid.Parse("11111111-1111-1111-1111-111111111068"),
            Guid.Parse("11111111-1111-1111-1111-111111111069"),
            Guid.Parse("11111111-1111-1111-1111-111111111070"),
            Guid.Parse("11111111-1111-1111-1111-111111111071"),
            Guid.Parse("11111111-1111-1111-1111-111111111072"),
            Guid.Parse("11111111-1111-1111-1111-111111111073"),
            Guid.Parse("11111111-1111-1111-1111-111111111074"),
            Guid.Parse("11111111-1111-1111-1111-111111111075"),
            Guid.Parse("11111111-1111-1111-1111-111111111076"),
            Guid.Parse("11111111-1111-1111-1111-111111111077"),
            Guid.Parse("11111111-1111-1111-1111-111111111078"),
            Guid.Parse("11111111-1111-1111-1111-111111111079"),
            Guid.Parse("11111111-1111-1111-1111-111111111080"),
            Guid.Parse("11111111-1111-1111-1111-111111111081"),
            Guid.Parse("11111111-1111-1111-1111-111111111082"),
            Guid.Parse("11111111-1111-1111-1111-111111111083"),
            Guid.Parse("11111111-1111-1111-1111-111111111084"),
            Guid.Parse("11111111-1111-1111-1111-111111111085"),
            Guid.Parse("11111111-1111-1111-1111-111111111086"),
            Guid.Parse("11111111-1111-1111-1111-111111111087"),
            Guid.Parse("11111111-1111-1111-1111-111111111088"),
            Guid.Parse("11111111-1111-1111-1111-111111111089"),
            Guid.Parse("11111111-1111-1111-1111-111111111090"),
            Guid.Parse("11111111-1111-1111-1111-111111111091"),
            Guid.Parse("11111111-1111-1111-1111-111111111092"),
            Guid.Parse("11111111-1111-1111-1111-111111111093"),
            Guid.Parse("11111111-1111-1111-1111-111111111094"),
            Guid.Parse("11111111-1111-1111-1111-111111111095"),
            Guid.Parse("11111111-1111-1111-1111-111111111096"),
            Guid.Parse("11111111-1111-1111-1111-111111111097"),
            Guid.Parse("11111111-1111-1111-1111-111111111098"),
            Guid.Parse("11111111-1111-1111-1111-111111111099"),
            Guid.Parse("11111111-1111-1111-1111-111111111100")
        };

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
                    var imageName = $"mainImageName{i + 1}";
                    var baseImageName = $"imageName{i + 1}";
                    var imageNames = new List<string>
                    {
                        $"{baseImageName}-1",
                        $"{baseImageName}-2",
                        $"{baseImageName}-3"
                    };
                    var sku = $"SKU-{Guid.NewGuid()}";
                    var stockQuantity = _random.Next(50, 200);
                    var views = _random.Next(20, 100);
                    var createdAt = DateTime.UtcNow.AddDays(-_random.Next(1, 100));
                    var updatedAt = createdAt.AddDays(_random.Next(0, 30));

                    var product = new Product
                    {
                        Id = ProductGuids[i],
                        Name = productName,
                        Description = productDescription,
                        SortDescription = sortDescription,
                        Price = productPrice,
                        MainImageBaseName = imageName,
                        ImageBaseNames = imageNames,
                        Sku = sku,
                        Views = views,
                        StockQuantity = stockQuantity,
                        IsActive = _random.Next(0, 2) == 1,
                        CategoryId = category.Id,
                        Category = category,
                        Rating = Math.Round(_random.NextDouble() * 5, 1),
                        TotalVotes = _random.Next(0, 500),
                        SoldQuantity = _random.Next(0, 101),
                        CreatedAt = createdAt,
                        UpdateAt = updatedAt
                    };

                    _context.Products.Add(product);
                }

                _context.SaveChanges();
            }
        }
    }
}
