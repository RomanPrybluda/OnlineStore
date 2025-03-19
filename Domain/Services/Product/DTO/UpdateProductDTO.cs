using DAL;

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

        public void UpdateProduct(Product product)
        {
            product.Name = Name ?? product.Name;
            product.Description = Description ?? product.Description;
            product.Price = Price ?? product.Price;
            product.ImageUrl = ImageUrl ?? product.ImageUrl;
            product.Sku = Sku ?? product.Sku;
            product.StockQuantity = StockQuantity ?? product.StockQuantity;
            product.CategoryId = CategoryId ?? product.CategoryId;
            product.IsActive = IsActive ?? product.IsActive;
        }
    }

}
