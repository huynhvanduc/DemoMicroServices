using Contract.Common.Events;
using Contract.Common.Interfaces;
using Contract.Domain.Interfaces;
using Infrastructure.Extensions;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace Ordering.Infrastructure.Persistence;

public  class OrderContext : DbContext
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public OrderContext(DbContextOptions<OrderContext> options, IMediator mediator, ILogger logger) : base(options)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public DbSet<Order> Orders { get; set; }
    private List<BaseEvent> _baseEvents = new();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    private void SetBaseEventsBeforeSavechanges()
    {
        var domainEntities = ChangeTracker.Entries<IEventEntity>()
         .Select(x => x.Entity)
         .Where(x => x.DomainEvents().Any())
         .ToList();

        _baseEvents = domainEntities
            .SelectMany(x => x.DomainEvents())
            .ToList();

        domainEntities.ForEach(x => x.ClearDomainEvents());
    }

    public override  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetBaseEventsBeforeSavechanges();

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

        var result = base.SaveChangesAsync(cancellationToken);
        _mediator.DispatchDomainEventAsync(_baseEvents);
        return result;
    }
}
