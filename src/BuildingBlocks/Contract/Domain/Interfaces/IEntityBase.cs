namespace Contract.Domain.Interfaces;

public interface IEntityBase<T>
{
    T Id { get; set; }
}
