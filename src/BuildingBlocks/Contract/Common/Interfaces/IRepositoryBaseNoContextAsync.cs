using Contract.Domain;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Contract.Common.Interfaces;

public interface IRepositoryBaseAsync<T, K>
    where T : EntityBase<K>
{
    IQueryable<T> FindAll(bool trackChanges = false);
    Task<List<T>> FindAllAsync(bool trackChanges = false);

    IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] expressions);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties);

    Task<T?> GetByIdAync(K id);

    Task<T> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties);

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
