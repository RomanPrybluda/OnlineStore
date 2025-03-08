using DAL;

namespace Domain.Seeds
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

            var existingCategoryCount = _context.Categories.Count();

            if (existingCategoryCount < 5)
            {

                var categories = new List<Category>
                {
                    new Category { Name = "Chocolate Products" },
                    new Category { Name = "Caramel" },
                    new Category { Name = "Cookies and Biscuits" },
                    new Category { Name = "Chewing Candies" },
                    new Category { Name = "Jelly and Marmalade" }
                };

                foreach (var category in categories)
                {
                    _context.Categories.Add(category);
                }

                _context.SaveChanges();
            }
        }
    }
}
