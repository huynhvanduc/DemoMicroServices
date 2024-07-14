using Contract.Common.Interfaces;
using Customer.API;
using Customer.API.Persistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
Log.Information("Starting Customer API up");

try
{

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    builder.Services.AddDbContext<CustomerContext>(options =>
        options.UseNpgsql(connectionString));

    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>()
        .AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
        .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
        .AddScoped<ICustomerService, CustomerService>();

    var app = builder.Build();

    app.MapGet("/", () => "Welcome to customer");
    app.MapGet("/api/customers/", async (ICustomerService customerService) => await customerService.GetUsers());
    app.MapGet("/api/customers/{userName}", 
        async (string userName, ICustomerService customerService) =>
    {
        var customer = await customerService.GetCustomerByUserNameAsync(userName);
        if (customer == null)
            return Results.NotFound();

        return Results.Ok(customer);
    });
    app.MapPost("/api/customers/",
        async (Customer.API.Entities.Customer customer, ICustomerRepository customerRepository) =>
        {
            await customerRepository.CreateAsync(customer);
            customerRepository.SaveChangesAsync();
        });
    app.MapDelete("/api/customers/{id}",
       async (int id, ICustomerRepository customerRepository) =>
       {
           var customer = await customerRepository.FindByCondition(x => x.Id.Equals(id)).SingleOrDefaultAsync();
           if (customer is null)
               return Results.NotFound();

           await customerRepository.DeleteAsync(customer);
           await customerRepository.SaveChangesAsync();
           return Results.Ok(customer);
       });

    app.SeedCustomerData().Run();
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


