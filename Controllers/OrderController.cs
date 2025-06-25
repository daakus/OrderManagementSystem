using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IDiscountService _discountService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderService orderService,
            IDiscountService discountService,
            ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _discountService = discountService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new order with automatic discount application
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            try
            {
                // Fix: Correct the argument type passed to CalculateDiscountAsync
                var discount = await _discountService.CalculateDiscountAsync(order);
                order.DiscountedAmount = order.OriginalAmount - discount;

                var createdOrder = await _orderService.CreateOrderAsync(order);
                return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return BadRequest("Failed to create order");
            }
        }

        /// <summary>
        /// Retrieves an order by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        /// <summary>
        /// Updates the status of an order
        /// </summary>
        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderStatus newStatus)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, newStatus);
                return Ok(updatedOrder);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves order analytics
        /// </summary>
        [HttpGet("analytics")]
        [ProducesResponseType(typeof(OrderAnalytics), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderAnalytics()
        {
            var analytics = await _orderService.GetOrderAnalyticsAsync();
            return Ok(analytics);
        }
    }
}