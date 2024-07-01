using Contract.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Contract.Common.Interfaces;

public interface IRepositoryBaseAsync<T, K, TContext> : IRepositoryBaseAsync<T, K>
    where T : EntityBase<K>
    where TContext : DbContext
{
    #region Get
    IQueryable<T> FindAll(bool trackChanges = false);
    Task<List<T>> FindAllAsync(bool trackChanges = false);

    IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] expressions);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties);

    Task<T?> GetByIdAync(K id);

    Task<T> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties);
    #endregion

    #region Create
    void Create(T entity);
    Task<K> CreateAsync(T entity);
    IList<K> CreateList(IEnumerable<T> entities);
    Task<IList<K>> CreateListAsync(IEnumerable<T> entities);
    #endregion

    #region Update
    void Update(T entity);
    Task UpdateAsync(T entity);

    void UpdateList(IEnumerable<T> entites);

    Task UpdateListAsync(IEnumerable<T> entities);
    #endregion

    #region Delete
    void Delete(T entity);

    Task DeleteAsync(T entity);
    
    void DeleteList(IEnumerable<T> entities);

    Task DeleteListAsync(IEnumerable<T> entities);
    #endregion

    #region Transaction
    Task<IDbContextTransaction> BeginTransactionAsync();

    Task EnTransactionAsync();

    Task RollbackTransactionAsync();

    Task<int> SaveChangesAsync();
    #endregion
}


