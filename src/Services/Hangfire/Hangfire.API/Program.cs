using Common.Logging;
using Hangfire.API.Extensions;
using Infrastructure.Scheduled.Jobs;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);
Log.Information("Starting Jobs API up");

try
{
    builder.Host.AddAppConfigurations();
    // Add services to the container.
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.ConfigureServices();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHangfireServiceInfra();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseHangfireDashboard(builder.Configuration);
    app.UseEndpoints(endpoints =>
        endpoints.MapDefaultControllerRoute()
    );
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandlerd exception");
}
finally
{
    Log.Information("Shut down Inventory API complete");
    Log.CloseAndFlush();
}

