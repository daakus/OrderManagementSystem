using OrderManagementSystem.Models;
using OrderManagementSystem.Services;
using Xunit;

namespace OrderManagementSystem.Tests.Services
{
    public class DiscountServiceTests
    {
        private readonly DiscountService _discountService;

        public DiscountServiceTests()
        {
            _discountService = new DiscountService();
        }

        [Theory]
        [InlineData(CustomerSegment.Standard, 100, 5)] // 5% discount
        [InlineData(CustomerSegment.Premium, 100, 10)] // 10% discount
        [InlineData(CustomerSegment.VIP, 100, 15)] // 15% discount
        public async Task CalculateDiscount_AppliesCorrectSegmentDiscount(
            CustomerSegment segment, decimal amount, decimal expectedDiscount)
        {
            // Arrange
            var order = new Order
            {
                CustomerSegment = segment,
                OriginalAmount = amount
            };

            // Act
            var discount = await _discountService.CalculateDiscountAsync(order);

            // Assert
            Assert.Equal(expectedDiscount, discount);
        }

        [Fact]
        public async Task CalculateDiscount_AppliesExtraDiscountForLargeOrders()
        {
            // Arrange
            var order = new Order
            {
                CustomerSegment = CustomerSegment.Standard,
                OriginalAmount = 1000 // Should get extra 5% discount
            };

            // Act
            var discount = await _discountService.CalculateDiscountAsync(order);

            // Assert
            Assert.Equal(100, discount); // 10% total (5% standard + 5% large order)
        }
    }
}