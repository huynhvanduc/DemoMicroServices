using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    private readonly ILogger _logger;
    private readonly OrderContext _orderContext;

    public OrderContextSeed(ILogger logger, OrderContext orderContext)
    {
        _logger = logger;
        _orderContext = orderContext;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_orderContext.Database.IsSqlServer())
            {
                await _orderContext.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {

            _logger.Error(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        if (!_orderContext.Orders.Any())
        {
            await _orderContext.Orders.AddRangeAsync(
                new Order
                {
                    EmailAddress = "customer1@gmail.com",
                    UserName = "customer1",
                    FirstName = "duc",
                    LastName = "duc",
                    ShippingAddress = "Da nang",
                    InvoiceAddress = "VN",
                    TotalPrice = 333,
                });

            await _orderContext.SaveChangesAsync();
        }
    }
}
