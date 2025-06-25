using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public class DiscountService : IDiscountService
    {
        public async Task<decimal> CalculateDiscountAsync(Order order)
        {
            // Use Task.Run to ensure the method runs asynchronously
            return await Task.Run(() =>
            {
                decimal discountPercentage = order.CustomerSegment switch
                {
                    CustomerSegment.VIP => 0.15m,        // 15% discount for VIP
                    CustomerSegment.Premium => 0.10m,     // 10% discount for Premium
                    CustomerSegment.Standard => 0.05m,    // 5% discount for Standard
                    _ => 0m
                };

                // Additional discount for orders over $1000
                if (order.OriginalAmount >= 1000)
                {
                    discountPercentage += 0.05m;
                }

                return order.OriginalAmount * discountPercentage;
            });
        }

        public async Task<decimal> CalculateDiscountAsync(IOrderService orderService, Guid orderId)
        {
            // Retrieve the order using the provided orderId  
            var order = await orderService.GetOrderAsync(orderId);
            return await CalculateDiscountAsync(order);
        }
    }
}