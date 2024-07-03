using MediatR;
using Ordering.Application.Features.V1.Orders.Commands;
using Ordering.Domain.OrderAgggregate.Events;
using Serilog;

namespace Ordering.Application.Features.V1.Orders;

public class OrderDomainHandler :
    INotificationHandler<OrderCreatedEvent>,
    INotificationHandler<OrderDeleteEvent>
{
    private readonly ILogger _logger;

    public OrderDomainHandler(ILogger logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.Warning("Ordering Domain Event Created: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }

    public Task Handle(OrderDeleteEvent notification, CancellationToken cancellationToken)
    {
        _logger.Warning("Ordering Domain Event Delete: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
