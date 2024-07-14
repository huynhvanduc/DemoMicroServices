using Common.Logging;
using Ocelot.DependencyInjection;
using Serilog;

namespace AcelotApiGw.Extensions;

public static class HostExtensions
{
    public static void AddAppConfigurations(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((context, config) =>
        {
            var env = context.HostingEnvironment;
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddJsonFile("ocelot.Development.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
        });
    }

    public static void AddConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOcelot(configuration);
    }

    public static void AddConfigCors(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration["AllowOrigins"];
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
    }
}
