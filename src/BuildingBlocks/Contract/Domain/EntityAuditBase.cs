using Contract.Domain.Interfaces;

namespace Contract.Domain;

public class EntityAuditBase<TKey> : EntityBase<TKey>, IAutditable
{
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset? LastModifiedTime { get; set; }
}
