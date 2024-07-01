using Contract.Common.Events;
using Contract.Domain.Interfaces;

namespace Contract.Common.Interfaces;

public interface IEventEntity
{
    void AddDomainEvent(BaseEvent domainEvent);
    void RemoveDomainEvent(BaseEvent domainEvent);
    void ClearDomainEvents();
    IReadOnlyCollection<BaseEvent> DomainEvents();
}

public interface IEventEntity<T> : IEntityBase<T>, IEventEntity
{
}