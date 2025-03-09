using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class CategoryService
    {
        private readonly OnlineStoreDbContext _context;

        public CategoryService(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            if (!categories.Any())
                throw new CustomException(CustomExceptionType.NotFound, "No categories found");

            return categories.Select(c => new CategoryDTO { Id = c.Id, Name = c.Name }).ToList();
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No category found with ID {id}");

            return new CategoryDTO { Id = category.Id, Name = category.Name };
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO request)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == request.Name);

            if (existingCategory != null)
                throw new CustomException(CustomExceptionType.IsAlreadyExists, $"Category '{request.Name}' already exists.");

            var category = new Category { Name = request.Name };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDTO { Id = category.Id, Name = category.Name };
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(Guid id, UpdateCategoryDTO request)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No category found with ID {id}");

            category.Name = request.Name;
            await _context.SaveChangesAsync();

            return new CategoryDTO { Id = category.Id, Name = category.Name };
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No category found with ID {id}");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
