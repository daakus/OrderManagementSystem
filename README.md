# Order Management System API

A .NET 8 Web API project that implements an order management system with features for discount calculation, order status tracking, and analytics.

## Project Overview

This system provides REST API endpoints for managing orders with the following key features:
- Customer segment-based discount system
- Order status tracking with state transitions
- Order analytics including average values and fulfillment times
- Comprehensive unit and integration tests

## Architecture

### Models

#### `Order`
The core entity representing an order in the system:
- `Id`: Unique identifier
- `CustomerId`: Customer's unique identifier
- `OriginalAmount`: Pre-discount order amount
- `DiscountedAmount`: Final amount after applying discounts
- `Status`: Current order status
- `CustomerSegment`: Customer's segment (Standard/Premium/VIP)
- `CreatedAt`: Order creation timestamp
- `LastModifiedAt`: Last modification timestamp
- `Items`: Collection of order items

#### `OrderItem`
Represents individual items within an order:
- `Id`: Unique identifier
- `OrderId`: Associated order ID
- `ProductName`: Name of the product
- `UnitPrice`: Price per unit
- `Quantity`: Number of units ordered

#### `CustomerSegment`
Enum defining customer segments:
- `Standard`
- `Premium`
- `VIP`

#### `OrderStatus`
Enum defining possible order states:
- `Created`
- `Confirmed`
- `Processing`
- `Shipped`
- `Delivered`
- `Cancelled`

#### `OrderAnalytics`
Data transfer object for order analytics:
- `AverageOrderValue`
- `AverageFulfillmentTime`
- `TotalOrders`
- `OrdersByStatus`

### Services

#### `IDiscountService` & `DiscountService`
Handles discount calculations based on:
- Customer segment (5% Standard, 10% Premium, 15% VIP)
- Order value (additional 5% for orders over $1000)

#### `IOrderService` & `OrderService`
Manages order operations:
- Order creation and retrieval
- Status updates with transition validation
- Analytics calculation
- In-memory order storage (for demonstration purposes)

### Controllers

#### `OrderController`
Provides REST API endpoints:
- POST `/api/order`: Create new order
- GET `/api/order/{id}`: Retrieve order by ID
- PUT `/api/order/{id}/status`: Update order status
- GET `/api/order/analytics`: Get order analytics

## Testing

### Unit Tests
Located in `Tests/OrderManagementSystem.Tests/Services`:
- `DiscountServiceTests`: Validates discount calculations for different scenarios
- Tests customer segment discounts
- Tests large order additional discounts

### Integration Tests
Located in `Tests/OrderManagementSystem.Tests/Integration`:
- `OrderControllerTests`: End-to-end API testing
- Tests order creation
- Tests status updates
- Tests analytics retrieval
