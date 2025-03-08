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
                    new Category { Id = Guid.NewGuid(), Name = "Candies" },
                    new Category { Id = Guid.NewGuid(), Name = "Chocolates" },
                    new Category { Id = Guid.NewGuid(), Name = "Cakes" },
                    new Category { Id = Guid.NewGuid(), Name = "Cookies" },
                    new Category { Id = Guid.NewGuid(), Name = "Pastries" },
                };

                _context.Categories.AddRange(categories);
                _context.SaveChanges();
            }
        }
    }
}
