using Contract.Common.Events;
using MediatR;

namespace Infrastructure.Extensions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventAsync(this IMediator mediator, List<BaseEvent> baseEvents)
    {
        foreach(var domainEvent in baseEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}