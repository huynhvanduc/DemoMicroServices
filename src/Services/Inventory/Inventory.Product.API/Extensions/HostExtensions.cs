using Inventory.Product.API.Persistence;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Product.API.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var sevices = scope.ServiceProvider;
        var settings = sevices.GetService<MongDbSettings>();
        if(settings == null || string.IsNullOrEmpty(settings.ConnectionString))
            throw new ArgumentNullException(nameof(settings) + "is not configured");

        var mongoClient = sevices.GetRequiredService<IMongoClient>();

        new InventoryDbSeed()
            .SeedDataAsync(mongoClient, settings)
            .Wait();

        return host;
    }
}
