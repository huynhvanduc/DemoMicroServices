using Contract.Domain.Interfaces;

namespace Contract.Common.Events;

public abstract class AuditableEventEntity<T> : EventEntity<T>, IAutditable
{
    public DateTimeOffset CreatedTime {  get; set; }
    public DateTimeOffset? LastModifiedTime { get; set; }
}
