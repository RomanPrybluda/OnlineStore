using DAL;
using Microsoft.AspNetCore.Http;

namespace Domain
{
    public class UpdateProductDTO
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? Sku { get; set; }

        public int? StockQuantity { get; set; }

        public Guid? CategoryId { get; set; }

        public bool? IsActive { get; set; }

        public IFormFile? MainProductImage { get; set; }

        public List<IFormFile>? ProductImages { get; set; }

        public void UpdateProduct(
            Product product,
            string imageBaseName,
            List<string> imageBaseNames)
        {
            product.Name = Name ?? product.Name;
            product.Description = Description ?? product.Description;
            product.Price = Price ?? product.Price;
            product.MainImageBaseName = ImageUrl ?? product.MainImageBaseName;
            product.Sku = Sku ?? product.Sku;
            product.StockQuantity = StockQuantity ?? product.StockQuantity;
            product.CategoryId = CategoryId ?? product.CategoryId;
            product.IsActive = IsActive ?? product.IsActive;
            product.MainImageBaseName = imageBaseName;
            product.ImageBaseNames = imageBaseNames;
        }
    }

}
