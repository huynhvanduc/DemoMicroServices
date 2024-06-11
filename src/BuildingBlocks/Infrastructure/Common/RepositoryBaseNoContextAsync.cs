using Contract.Common.Interfaces;
using Contract.Domain;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Infrastructure.Common;

public class RepositoryBaseNoContextAsync<T, K> :
    IRepositoryBaseAsync<T, K>
    where T : EntityBase<K>
{
    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<K> CreateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<IList<K>> CreateListAsync(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteListAsync(T entities)
    {
        throw new NotImplementedException();
    }

    public Task EnTransactionAsync()
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> FindAll(bool trackChanges = false)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] expressions)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> FindAllAsync(bool trackChanges = false)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetByIdAync(K id)
    {
        throw new NotImplementedException();
    }

    public Task RollbackTransactionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateListAsync(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }
}
