using Product.API.Entities;
using ILogger = Serilog.ILogger;

namespace Product.API.Persistence;

public static class ContextSeed
{
    public static async Task SeedProductAsync(ProductContext productContext, ILogger logger)
    {
        if (!productContext.Products.Any())
        {
            productContext.AddRange(GetCatalogProduct());
            await productContext.SaveChangesAsync();
        }
    }

    private static IEnumerable<CatalogProduct> GetCatalogProduct()
    {
        return new List<CatalogProduct>
        {
            new()
            {
                No = "Lotus",
                Name = "Esprit",
                Sumary = ",sfdmfdm",
                Price = (decimal) 22.33,
            },
            new()
            {
                No = "Xelo",
                Name = "Huhu",
                Sumary = ",sfdmfdm_hgh",
                Price = (decimal) 22.34
            }
        };
    }
}
