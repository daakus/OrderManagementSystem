using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderManagementSystem.Models;
using Xunit;

namespace OrderManagementSystem.Tests.Integration
{
    public class OrderControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public OrderControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedOrder()
        {
            // Arrange
            var order = new Order
            {
                CustomerId = Guid.NewGuid(),
                CustomerSegment = CustomerSegment.Premium,
                OriginalAmount = 100m,
                Items = new List<OrderItem>
                {
                    new() { ProductName = "Test Product", UnitPrice = 100m, Quantity = 1 }
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/order", order);
            var createdOrder = await response.Content.ReadFromJsonAsync<Order>();

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(createdOrder);
            Assert.NotEqual(Guid.Empty, createdOrder.Id);
            Assert.Equal(90m, createdOrder.DiscountedAmount); // 10% Premium discount
        }

        [Fact]
        public async Task UpdateOrderStatus_WithValidTransition_ReturnsUpdatedOrder()
        {
            // Arrange
            var order = new Order
            {
                CustomerId = Guid.NewGuid(),
                CustomerSegment = CustomerSegment.Standard,
                OriginalAmount = 100m
            };
            var createResponse = await _client.PostAsJsonAsync("/api/order", order);
            var createdOrder = await createResponse.Content.ReadFromJsonAsync<Order>();

            // Act
            var response = await _client.PutAsJsonAsync(
                $"/api/order/{createdOrder!.Id}/status", 
                OrderStatus.Confirmed);
            var updatedOrder = await response.Content.ReadFromJsonAsync<Order>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(updatedOrder);
            Assert.Equal(OrderStatus.Confirmed, updatedOrder.Status);
        }

        [Fact]
        public async Task GetOrderAnalytics_ReturnsAnalytics()
        {
            // Act
            var response = await _client.GetAsync("/api/order/analytics");
            var analytics = await response.Content.ReadFromJsonAsync<OrderAnalytics>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(analytics);
        }
    }
}