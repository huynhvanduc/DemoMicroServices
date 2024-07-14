using Common.Logging;
using Inventory.Grpc;
using Inventory.Grpc.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    Log.Information($"Start {builder.Environment.ApplicationName} up");

    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.ConfigureMongDbClient();
    builder.Services.AddInfrastructureServices();

    builder.Services.AddGrpc();

    var app = builder.Build();
    app.MapGrpcService<InventoryService>();
    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
    throw;
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}
