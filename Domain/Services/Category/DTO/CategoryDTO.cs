using DAL;

namespace Domain
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public static Category ToCategory(CreateCategoryDTO request)
        {
            return new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description
            };
        }
    }
}
