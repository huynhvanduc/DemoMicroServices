using Basket.API;
using Basket.API.Extensions;
using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Information("Starting Basket API up");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Host.AddAppConfigurations();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.ConfigureHttpClientService();
    builder.Services.AddAutoMapper(cfg => {
        cfg.AddProfile(new MappingProfile());
    });
    builder.Services.ConfigureServices();
    builder.Services.ConfigureRedis(builder.Configuration);
    builder.Services.ConfigureGrpcSerivces();
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

    builder.Services.ConfigureMassTransit();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    var app = builder.Build();
    app.UseInfrastructure();
    app.Run();

    
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostExeption", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Unhandlerd exception");
}
finally
{
    Log.Information("Shut down Basket API complete");
    Log.CloseAndFlush();
}


