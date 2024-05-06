using Contract.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;

namespace Product.API.Persistence;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
        
    }

    public DbSet<Entities.CatalogProduct> Products { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var modified = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified
                || e.State == EntityState.Added
                || e.State == EntityState.Deleted);

        foreach(var item in modified)
        {
            switch (item.State)
            {
                case EntityState.Added:
                    if(item.Entity is IDateTracking addedEntity)
                    {
                        addedEntity.CreatedTime = DateTime.UtcNow;
                        item.State = EntityState.Added;
                    }
                    break;

                case EntityState.Modified:
                    Entry(item.Entity).Property("Id").IsModified = false;
                    if(item.Entity is IDateTracking modifiedEntity)
                    {
                        modifiedEntity.LastModifiedTime = DateTime.UtcNow;
                        item.State = EntityState.Modified;
                    }
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
