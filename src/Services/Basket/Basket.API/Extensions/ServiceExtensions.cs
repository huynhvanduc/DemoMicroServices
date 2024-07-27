using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services;
using Basket.API.Services.Interfaces;
using Contract.Common.Interfaces;
using EvenBus.Messages.IntegrationEvent.Interfaces;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Inventory.Grpc.Protos;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Configurations;

namespace Basket.API.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
            .Get<EventBusSettings>();

        var cacheSettings = configuration.GetSection(nameof(CacheSettings))
            .Get<CacheSettings>();

        var grpcSettings = configuration.GetSection(nameof(GrpcSettings))
            .Get<GrpcSettings>();

        var backgroundJobSettings = configuration.GetSection(nameof(BackgroundJobSettings))
            .Get<BackgroundJobSettings>();

        services.AddSingleton(eventBusSettings);
        services.AddSingleton(cacheSettings);
        services.AddSingleton(grpcSettings);
        services.AddSingleton(backgroundJobSettings);

        return services;
    }

    public static void ConfigureHttpClientService(this IServiceCollection services) {
        services.AddHttpClient<BackgroundJobHttpService>();
    }

    public static IServiceCollection ConfigureGrpcSerivces(this IServiceCollection services)
    {
        var settings = services.GetOptions<GrpcSettings>(nameof(GrpcSettings));
        services.AddGrpcClient<StockProtoService.StockProtoServiceClient>(x => x.Address = new Uri(settings.StockUrl));
        services.AddScoped<StockItemGrpcService>();

        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
        => services.AddScoped<IBasketRepository, BasketRepository>()
        .AddTransient<ISerializeService, SerializeService>()
        .AddTransient<IEmailTemplateServices, BasketEmailTemplateService>()
        ;

    public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = services.GetOptions<CacheSettings>(nameof(CacheSettings));
        if (string.IsNullOrEmpty(settings.ConnectionString))
            throw new ArgumentNullException("Redis Connection string is not configured");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = settings.ConnectionString;
        });
    }

    public static void ConfigureMassTransit(this IServiceCollection services)
    {
        var settings = services.GetOptions<EventBusSettings>(nameof(EventBusSettings));
        if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
            throw new ArgumentNullException("EventBusSettings is not configured");

        var mqConnection = new Uri(settings.HostAddress);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(mqConnection);
            });
            config.AddRequestClient<IBasketCheckoutEvent>();
        });
    }
}
