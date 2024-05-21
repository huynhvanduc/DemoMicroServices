using Common.Logging;
using Product.API.Extensions;
using Product.API.Persistence;
using Serilog;

Log.Information("Starting Product API up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    //logging
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    app.UseInfrastructure();
    app.MigrateDatabase<ProductContext>((context, _) =>
    {
        ContextSeed.SeedProductAsync(context, Log.Logger).Wait();
    })
        .Run();
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


