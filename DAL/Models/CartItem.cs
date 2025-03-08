namespace DAL
{
    public class CartItem
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        public string UserId { get; set; } = string.Empty;

    }
}
