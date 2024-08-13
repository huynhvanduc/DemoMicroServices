﻿using Basket.API.Entities;
using Contract.Sagas.OrderManager;
using Saga.Orchestrator.HttpRepository;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Saga.Orchestrator.OrderManager;
using Saga.Orchestrator.Services;
using Saga.Orchestrator.Services.Interfaces;

namespace Saga.Orchestrator.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureService(this IServiceCollection services) 
        => services.AddTransient<ICheckoutSagaService, CheckoutSagaService>()
        .AddTransient<ISagaOrderManager<BasketCheckoutDto, OrderResponse>, SagaOrderManager>();

    public static IServiceCollection ConfigureHttpRepository(this IServiceCollection services)
        => services.AddScoped<IOrderHttpRepository, OrderHttpRepository>()
        .AddScoped<IBasketHttpRepository, BasketHttpRepository>()
        .AddScoped<IInventoryHttpRepository, InventoryHttpRepository>();

    public static void ConfigureHttpClients(this IServiceCollection services)
    {
        ConfigureOrderHttpClient(services);
        ConfigureBasketHttpClient(services);
        ConfigureInventoryHttpClient(services);
    }

    private static void ConfigureOrderHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IOrderHttpRepository, OrderHttpRepository>("OrdersAPI", (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri("http://localhost:5005/api/v1/");
        });
        services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
        .CreateClient("OrdersAPI"));
    }

    private static void ConfigureBasketHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IBasketHttpRepository, BasketHttpRepository>("BasketAPI", (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri("http://localhost:5004/api/");
        });
        services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
        .CreateClient("BasketAPI"));
    }

    private static void ConfigureInventoryHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IInventoryHttpRepository, InventoryHttpRepository>("InventoryAPI", (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri("http://localhost:5006/api/");
        });
        services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
        .CreateClient("InventoryAPI"));
    }
}
