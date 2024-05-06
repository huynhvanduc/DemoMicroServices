using Microsoft.EntityFrameworkCore;

namespace Contract.Common.Interfaces;

public interface IUnitOfWork<TContext> : IDisposable
    where TContext : DbContext
{
    Task<int> CommitAsync();
}
