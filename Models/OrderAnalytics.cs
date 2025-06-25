namespace OrderManagementSystem.Models
{
    public class OrderAnalytics
    {
        public decimal AverageOrderValue { get; set; }
        public TimeSpan AverageFulfillmentTime { get; set; }
        public int TotalOrders { get; set; }
        public Dictionary<OrderStatus, int> OrdersByStatus { get; set; } = new();
    }
}