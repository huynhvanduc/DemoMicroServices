using AutoMapper;
using Common.Logging;
using Saga.Orchestrator;
using Saga.Orchestrator.Extensions;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(Serilogger.Configure);
    Log.Information($"start {builder.Environment.ApplicationName} up");

    builder.Host.AddAppConfigurations();
    builder.Services.ConfigureService();
    builder.Services.ConfigureHttpRepository();
    builder.Services.ConfigureHttpClients();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.Configure<RouteOptions>(options =>
        options.LowercaseUrls = true);
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

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
    app.MapControllers();
    app.UseEndpoints(endpoints =>
        endpoints.MapDefaultControllerRoute()
    );
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
    Log.Information("Shut down Product API complete");
    Log.CloseAndFlush();
}


