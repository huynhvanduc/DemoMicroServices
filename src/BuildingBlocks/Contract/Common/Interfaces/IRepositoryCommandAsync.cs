using Contract.Domain;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Contract.Common.Interfaces;

public interface IRepositoryCommandAsync<T, K, TContext> : IRepositoryBaseAsync<T, K, TContext>
where T : EntityBase<K>
where TContext : DbContext
{
    Task<K> CreateAsync(T entity);

    Task<IList<K>> CreateListAsync(IEnumerable<T> entities);

    Task UpdateAsync(T entity);

    Task UpdateListAsync(IEnumerable<T> entities);

    Task<int> SaveChangesAsync();

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task EnTransactionAsync();

    Task RollbackTransactionAsync();

    Task DeleteAsync(T entity);

    Task DeleteListAsync(T entities);
}
