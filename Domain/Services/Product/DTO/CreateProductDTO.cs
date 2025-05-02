using DAL;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class CreateProductDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be not zero and be positive!")]
        public decimal Price { get; set; }

        public string Sku { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int StockQuantity { get; set; }

        public Guid CategoryId { get; set; }

        public IFormFile MainProductImage { get; set; } = null!;

        public List<IFormFile> ProductImages { get; set; } = null!;


        public static Product ToProduct(
            CreateProductDTO request,
            string imageBaseName,
            List<string> imageBaseNames)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                MainImageBaseName = imageBaseName,
                ImageBaseNames = imageBaseNames,
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
