using Contract.Common.Interfaces;
using Contract.Domain;

namespace Contract.Common.Events;

public class EventEntity<T> : EntityBase<T>, IEventEntity
{
    private List<BaseEvent> _domainEvents = new();
    
    public IReadOnlyCollection<BaseEvent> DomainEvents() => _domainEvents.AsReadOnly();
  
    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
 
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }


    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }
}
