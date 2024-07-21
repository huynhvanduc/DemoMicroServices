using AcelotApiGw.Extensions;
using Common.Logging;
using Infrastructure.Middlewares;
using Ocelot.Middleware;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting API Gateway up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.AddAppConfigurations();
    builder.Host.UseSerilog(Serilogger.Configure);


    // Add services to the container.
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    builder.Services.AddConfigCors(builder.Configuration);

    builder.Services.ConfigureOcelot(builder.Configuration);

    builder.Services.ConfigureCors(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        //app.UseSwaggerUI();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", 
        $"{builder.Environment.ApplicationName} v1"));
    }

    //app.UseCors("CorsPolicy");
    app.UseMiddleware<ErrorWrappingMiddleware>();
    app.UseAuthentication();
    app.UseRouting();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGet("/", context =>
        {
            // await context.Response.WriteAsync($"Hello TEDU members! This is {builder.Environment.ApplicationName}");
            context.Response.Redirect("swagger/index.html");
            return Task.CompletedTask;
        });
    });

    //app.UseHttpsRedirection();
    app.MapControllers();
    await app.UseOcelot();
    app.Run();
}
catch (Exception ex)
{
    
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;
    Log.Fatal(ex, $"{ex.Message}");
}
finally
{
    Log.Information("Shut down Ordering API complete");
    Log.CloseAndFlush();
}