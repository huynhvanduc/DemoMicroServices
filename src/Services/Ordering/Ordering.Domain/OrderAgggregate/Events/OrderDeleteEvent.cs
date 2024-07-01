using Contract.Common.Events;

namespace Ordering.Domain.OrderAgggregate.Events;

public class OrderDeleteEvent : BaseEvent
{
    public long Id { get; }

    public OrderDeleteEvent(long id)
    {
        Id = id;
    }
}
