using Contract.Common.Interfaces;
using Contract.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Common;

public class RepositoryCommandAsync<T, K, TContext> : RepositoryBaseAsync<T, K, TContext>,
    IRepositoryCommandAsync<T, K, TContext>
    where T : EntityBase<K>
    where TContext : DbContext
{
    public RepositoryCommandAsync(TContext dbContext, IUnitOfWork<TContext> uniUnitOfWork) : base(dbContext, uniUnitOfWork)
    {
    }

    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return _dbContext.Database.BeginTransactionAsync();
    }

    public async Task<K> CreateAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity.Id;
    }

    public async Task<IList<K>> CreateListAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        return entities.Select(x => x.Id).ToList();
    }

    public Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteListAsync(T entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public async Task EnTransactionAsync()
    {
        await SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _dbContext.Database.RollbackTransactionAsync();
    }

    public Task<int> SaveChangesAsync()
    {
        return _uniUnitOfWork.CommitAsync();
    }

    public Task UpdateAsync(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Unchanged)
            return Task.CompletedTask;

        T exist = _dbContext.Set<T>().Find(entity.Id);
        _dbContext.Entry(exist).CurrentValues.SetValues(entity);

        return Task.CompletedTask;
    }

    public Task UpdateListAsync(IEnumerable<T> entities)
    {
        return _dbContext.Set<T>().AddRangeAsync(entities);
    }
}
