using Contract.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using System.Reflection;

namespace Ordering.Infrastructure.Persistence;

public  class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {

    }

    DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var modified = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified
                || e.State == EntityState.Added
                || e.State == EntityState.Deleted);

        foreach (var item in modified)
        {
            switch (item.State)
            {
                case EntityState.Added:
                    if (item.Entity is IDateTracking addedEntity)
                    {
                        addedEntity.CreatedTime = DateTime.UtcNow;
                        item.State = EntityState.Added;
                    }
                    break;

                case EntityState.Modified:
                    Entry(item.Entity).Property("Id").IsModified = false;
                    if (item.Entity is IDateTracking modifiedEntity)
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
