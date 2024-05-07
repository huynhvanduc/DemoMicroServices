using Contract.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;
using Product.API.Repositories;
using Product.API.Repositories.Interfaces;
using System.Runtime.CompilerServices;

namespace Product.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ConfigureProductDbContext(configuration);
        services.AddInfrastructureServices();
        return services;
    }
    private static IServiceCollection ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        var builder = new MySqlConnectionStringBuilder(connectionString);

        IServiceCollection serviceCollection = services.AddDbContext<ProductContext>(option => 
        option.UseMySql(builder.ConnectionString,
            ServerVersion.AutoDetect(builder.ConnectionString),
            e =>
            {
                e.MigrationsAssembly("Product.API");
                e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            }));

        return services;
    }

    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        => services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
        .AddScoped(typeof(IRepositoryCommandAsync<,,>), typeof(RepositoryCommandAsync<,,>))
        .AddScoped(typeof(IProductRepository), typeof(ProductRepository))
        .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
}
