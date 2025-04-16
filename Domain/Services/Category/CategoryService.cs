using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class CategoryService
    {
        private readonly OnlineStoreDbContext _context;
        private readonly ImageService _imageService;

        public CategoryService(OnlineStoreDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            if (!categories.Any())
                throw new CustomException(CustomExceptionType.NotFound,
                    "No categories found");

            var categoryDTOs = new List<CategoryDTO>();

            foreach (var category in categories)
            {
                var categoryDTO = CategoryDTO.FromCategory(category);
                categoryDTOs.Add(categoryDTO);
            }

            return categoryDTOs;
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(Guid id)
        {
            var categoryById = await _context.Categories.FindAsync(id);

            if (categoryById == null)
                throw new CustomException(CustomExceptionType.NotFound,
                    $"No category found with ID {id}");

            var categoryDTO = CategoryDTO.FromCategory(categoryById);

            return categoryDTO;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO request)
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == request.Name);

            if (existingCategory != null)
                throw new CustomException(CustomExceptionType.IsAlreadyExists,
                    $"Category '{request.Name}' already exists.");

            string imageBaseName = string.Empty;
            if (request.Image != null)
                imageBaseName = await _imageService.UploadImageAsync(request.Image);

            var category = CreateCategoryDTO.ToCategory(request, imageBaseName);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var createdCategory = await _context.Categories.FindAsync(category.Id);

            if (createdCategory == null)
                throw new CustomException(CustomExceptionType.NotFound,
                    "The category could not be found after creation.");

            var categoryDTO = CategoryDTO.FromCategory(createdCategory);

            return categoryDTO;
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(Guid id, UpdateCategoryDTO request)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                throw new CustomException(CustomExceptionType.NotFound,
                    $"No category found with ID {id}");

            string imageUrl = string.Empty;
            if (request.Image != null)
                imageUrl = await _imageService.UploadImageAsync(request.Image);

            request.UpdateCategory(category, imageUrl);

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            var categoryDTO = CategoryDTO.FromCategory(category);

            return categoryDTO;
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                throw new CustomException(CustomExceptionType.NotFound,
                    $"No category found with ID {id}");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
