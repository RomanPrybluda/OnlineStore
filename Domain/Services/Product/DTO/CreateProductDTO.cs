using DAL;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class CreateProductDTO
    {
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Short description is required.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "Short description must be between 5 and 100 characters.")]
        public string SortDescription { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be not zero and be positive!")]
        public decimal Price { get; set; }

        public string Sku { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be non-negative!")]
        public int StockQuantity { get; set; }

        public Guid CategoryId { get; set; }


        public static Product ToProduct(
            CreateProductDTO request
            )
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                SortDescription = request.SortDescription,
                Price = request.Price,
                Sku = request.Sku,
                StockQuantity = request.StockQuantity,
                IsActive = request.IsActive,
                CategoryId = request.CategoryId,
                Rating = 0,
                TotalVotes = 0
            };
        }
    }
}
