namespace DAL
{
    public class ProductDetails
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public string Sku { get; set; } = string.Empty;

        public string Attributes { get; set; } = string.Empty;

        public int StockQuantity { get; set; }
    }
}
