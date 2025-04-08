using DAL;
using Microsoft.AspNetCore.Http;

namespace Domain
{
    public class UpdateCategoryDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public IFormFile Image { get; set; }

        public void UpdateCategory(Category category, string imageUrl)
        {
            category.Name = Name ?? category.Name;
            category.Description = Description ?? category.Description;
            category.ImageUrl = imageUrl;
        }

    }
}
