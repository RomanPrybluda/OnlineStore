namespace DAL
{
    public class CartItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        public string UserId { get; set; } = string.Empty;

    }
}
