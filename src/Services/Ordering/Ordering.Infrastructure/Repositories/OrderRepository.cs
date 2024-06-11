using Contract.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBaseAsync<Order, long, OrderContext>, IOrderRepository
{
    public OrderRepository(OrderContext dbContext, IUnitOfWork<OrderContext> uniUnitOfWork) : base(dbContext, uniUnitOfWork)
    {
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserName(string name) 
        => await FindByCondition(x => x.UserName.Equals(name)).ToListAsync();
}
