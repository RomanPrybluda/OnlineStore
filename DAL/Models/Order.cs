namespace DAL
{
    public class Order
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
