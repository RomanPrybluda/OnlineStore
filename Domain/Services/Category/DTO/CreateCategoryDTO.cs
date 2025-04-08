using DAL;
using Microsoft.AspNetCore.Http;

namespace Domain
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public IFormFile Image { get; set; }

        public static Category ToCategory(CreateCategoryDTO request, string imageUrl)
        {
            return new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                ImageUrl = imageUrl
            };
        }
    }
}
