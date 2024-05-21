using Contract.Domain.Interfaces;

namespace Contract.Domain;

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    public TKey Id { get; set; }
}
