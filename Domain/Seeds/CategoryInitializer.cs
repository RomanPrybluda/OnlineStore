using DAL;

namespace Domain
{
    public class CategoryInitializer
    {
        private readonly OnlineStoreDbContext _context;

        public CategoryInitializer(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public void InitializeCategories()
        {
            if (!_context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Id = Guid.Parse("22222222-1111-1111-1111-111111111001"),
                        Name = "Cakes",
                        Description = "Freshly baked cakes with various flavors",
                        ImageBaseName = "cakes"
                    },
                    new Category
                    {
                        Id = Guid.Parse("22222222-1112-1111-1111-111111111002"),
                        Name = "Boxes sweets",
                        Description = "Assorted sweets in elegant gift boxes",
                        ImageBaseName = "boxes_sweets"
                    },
                    new Category
                    {
                        Id = Guid.Parse("22222222-1113-1111-1111-111111111003"),
                        Name = "Caramel and candies",
                        Description = "A variety of caramel and hard candies",
                        ImageBaseName = "caramel_candies"
                    },
                    new Category
                    {
                        Id = Guid.Parse("22222222-1114-1111-1111-111111111004"),
                        Name = "Chocolate bar",
                        Description = "Delicious chocolate bars with different fillings",
                        ImageBaseName = "chocolate_bar"
                    },
                    new Category
                    {
                        Id = Guid.Parse("22222222-1115-1111-1111-111111111005"),
                        Name = "Biscuits",
                        Description = "Crunchy and flavorful biscuits",
                        ImageBaseName = "biscuits"
                    },
                    new Category
                    {
                        Id = Guid.Parse("22222222-1116-1111-1111-111111111006"),
                        Name = "Marshmallow",
                        Description = "Soft and fluffy marshmallows",
                        ImageBaseName = "marshmallow"
                    },
                    new Category
                    {
                        Id = Guid.Parse("22222222-1117-1111-1111-111111111007"),
                        Name = "Healthy sweets",
                        Description = "Sweet treats made with natural ingredients",
                        ImageBaseName = "healthy_sweets"
                    },
                    new Category
                    {
                        Id = Guid.Parse("22222222-1118-1111-1111-111111111008"),
                        Name = "Gift sets",
                        Description = "Beautifully packaged sweet gift sets",
                        ImageBaseName = "gift_sets"
                    },
                };

                _context.Categories.AddRange(categories);
                _context.SaveChanges();
            }
        }
    }
}
