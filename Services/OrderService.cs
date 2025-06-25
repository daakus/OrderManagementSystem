using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public class OrderService : IOrderService
    {
        private static readonly Dictionary<Guid, Order> _orders = new();
        private readonly ILogger<OrderService> _logger;

        public OrderService(ILogger<OrderService> logger)
        {
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.Id = Guid.NewGuid();
            order.CreatedAt = DateTime.UtcNow;
            order.Status = OrderStatus.Created;

            _orders[order.Id] = order;
            _logger.LogInformation("Created order {OrderId}", order.Id);

            return await Task.FromResult(order);
        }

        public async Task<Order?> GetOrderAsync(Guid id)
        {
            return await Task.FromResult(_orders.GetValueOrDefault(id));
        }

        public async Task<Order> UpdateOrderStatusAsync(Guid id, OrderStatus newStatus)
        {
            if (!_orders.TryGetValue(id, out var order))
            {
                throw new KeyNotFoundException($"Order {id} not found");
            }

            // Validate status transition
            bool isValidTransition = (order.Status, newStatus) switch
            {
                (OrderStatus.Created, OrderStatus.Confirmed) => true,
                (OrderStatus.Confirmed, OrderStatus.Processing) => true,
                (OrderStatus.Processing, OrderStatus.Shipped) => true,
                (OrderStatus.Shipped, OrderStatus.Delivered) => true,
                (_, OrderStatus.Cancelled) => true, // Can cancel from any state
                _ => false
            };

            if (!isValidTransition)
            {
                throw new InvalidOperationException(
                    $"Invalid status transition from {order.Status} to {newStatus}");
            }

            order.Status = newStatus;
            order.LastModifiedAt = DateTime.UtcNow;

            return await Task.FromResult(order);
        }

        public async Task<OrderAnalytics> GetOrderAnalyticsAsync()
        {
            var analytics = new OrderAnalytics
            {
                TotalOrders = _orders.Count,
                AverageOrderValue = _orders.Any()
                    ? _orders.Average(o => o.Value.DiscountedAmount)
                    : 0,
                OrdersByStatus = _orders
                    .GroupBy(o => o.Value.Status)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            // Calculate average fulfillment time for delivered orders
            var deliveredOrders = _orders.Values
                .Where(o => o.Status == OrderStatus.Delivered && o.LastModifiedAt.HasValue);

            analytics.AverageFulfillmentTime = deliveredOrders.Any()
                ? TimeSpan.FromTicks((long)deliveredOrders
                    .Average(o => (o.LastModifiedAt!.Value - o.CreatedAt).Ticks))
                : TimeSpan.Zero;

            return await Task.FromResult(analytics);
        }

        public Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }
    }
}