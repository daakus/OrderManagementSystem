using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order?> GetOrderAsync(Guid id);
        Task<Order> UpdateOrderStatusAsync(Guid id, OrderStatus newStatus);
        Task<OrderAnalytics> GetOrderAnalyticsAsync();
        Task<IEnumerable<Order>> GetAllOrdersAsync();
    }
}