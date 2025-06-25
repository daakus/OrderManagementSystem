namespace OrderManagementSystem.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public OrderStatus Status { get; set; }
        public CustomerSegment CustomerSegment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}