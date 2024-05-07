using Contract.Common.Interfaces;
using Contract.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Common;

public class RepositoryBaseAsync<T, K, TContext>
    : IRepositoryBaseAsync<T, K, TContext>
    where T : EntityBase<K>
    where TContext : DbContext
{

    protected readonly TContext _dbContext;
    protected readonly IUnitOfWork<TContext> _uniUnitOfWork;

    public RepositoryBaseAsync(TContext dbContext, IUnitOfWork<TContext> uniUnitOfWork)
    {
        _dbContext = dbContext;
        _uniUnitOfWork = uniUnitOfWork;
    }

    public IQueryable<T> FindAll(bool trackChanges = false) =>
    !trackChanges ? _dbContext.Set<T>().AsNoTracking() :
        _dbContext.Set<T>();

    public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
    !trackChanges
        ? _dbContext.Set<T>().Where(expression).AsNoTracking()
        : _dbContext.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public async Task<T?> GetByIdAync(K id)
    {
        return await FindByCondition(x => x.Id!.Equals(id), false)
             .FirstOrDefaultAsync();
    }

    public async Task<T?> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties)
    {
        return await FindByCondition(x => x.Id.Equals(id), false, includeProperties)
        .FirstOrDefaultAsync();
    }
}
