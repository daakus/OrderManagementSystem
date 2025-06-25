using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public interface IDiscountService
    {
        Task<decimal> CalculateDiscountAsync(Order order);
    }
}