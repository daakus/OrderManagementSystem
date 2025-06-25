using Microsoft.OpenApi.Models;
using OrderManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Order Management System API", 
        Version = "v1",
        Description = "API for managing orders with discount and status tracking features"
    });
});

// Register services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())    
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Make the Program class public for testing
public partial class Program { }
