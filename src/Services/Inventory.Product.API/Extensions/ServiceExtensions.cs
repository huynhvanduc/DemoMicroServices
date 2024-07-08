using Infrastructure.Extensions;
using Inventory.Product.API.Services;
using Inventory.Product.API.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Inventory.Product.API.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
    IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection(nameof(DatabaseSettings))
            .Get<DatabaseSettings>();

        services.AddSingleton(databaseSettings);

        return services;
    }

    private static string GetMongodbConnectionString(this IServiceCollection services)
    {
        var settings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
        if(settings == null || string.IsNullOrEmpty(settings.ConnectionString))
        {
            throw new ArgumentNullException("DatabaseSettings is not configured.");
        }

        var databaseName = settings.DatabaseName;
        var mongDbConnectionString = settings.ConnectionString + "/" + databaseName + "?authSource=admin";

        return mongDbConnectionString;
    }

    public static void ConfigureMongDbClient(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(
            new MongoClient(GetMongodbConnectionString(services)))
            .AddScoped(x => x.GetService<IMongoClient>()?.StartSession())
            ;


    }

    public static void AddInfrastructureSerivces(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        services.AddScoped<IInventoryService, InventorySerivce>();
    }
}
