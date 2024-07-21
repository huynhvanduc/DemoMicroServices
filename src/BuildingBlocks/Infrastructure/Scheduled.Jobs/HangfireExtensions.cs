using Hangfire;
using Hangfire.Console;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;
using Shared.Configurations;
using System.Security.Authentication;

namespace Infrastructure.Scheduled.Jobs;

public static class HangfireExtensions
{
    public static IServiceCollection AddHangfireServiceInfra(this IServiceCollection services)
    {
        var settings = services.GetOptions<HangfireSettings>(nameof(HangfireSettings));
        if (settings == null || settings.Storage == null || string.IsNullOrEmpty(settings.Storage.ConnectionString))
            throw new Exception("HangfireSettings is not configured properly!");

        services.ConfigureHangfireServices(settings);
        services.AddHangfireServer(serverOptions => { 
            serverOptions.ServerName = settings.ServerName; 
        });

        return services;
    }

    private static IServiceCollection ConfigureHangfireServices(this IServiceCollection services, HangfireSettings settings)
    {
        if (string.IsNullOrEmpty(settings.Storage.DBProvider))
            throw new Exception("HangFire DBProvider is not configured");

        switch (settings.Storage.DBProvider.ToLower())
        {
            case "mongodb":
                var mongUrlBuilder = new MongoUrlBuilder(settings.Storage.ConnectionString);
                var mongClientSettings = MongoClientSettings.FromUrl(
                    new MongoUrl(settings.Storage.ConnectionString));

                mongClientSettings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = SslProtocols.Tls12
                };
                var mongoClient = new MongoClient(mongClientSettings);

                var mongoStorageOptions = new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions { 
                       MigrationStrategy = new MigrateMongoMigrationStrategy(),
                       BackupStrategy = new CollectionMongoBackupStrategy()
                    },
                    CheckConnection = true,
                    Prefix = "ScheduleQueue",
                    CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
                };

                services.AddHangfire((provider, config) =>
                {
                    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseRecommendedSerializerSettings()
                    .UseConsole()
                    .UseMongoStorage(mongoClient, mongUrlBuilder.DatabaseName, mongoStorageOptions);

                    var jsonSettings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    };

                    config.UseSerializerSettings(jsonSettings);
                });
                break;

            case "postgresql":

                break;

            case "mssql":

                break;

            default:
                throw new Exception("HangFire Storage Provider {settings.Storage.DBProvider} is not supported");
        }

        return services;
    }
}
