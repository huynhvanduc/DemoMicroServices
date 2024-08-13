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

    public async Task<Order> CreateOrder(Order order)
    {
        await CreateAsync(order);
        return order;
    }

    public void DeleteOrder(Order order) => Delete(order);

    public async  Task<Order> GetOrdersByDocumentNo(string documentNo)
        => await  FindByCondition(x => x.DocumentNo.ToString().Equals(documentNo)).FirstOrDefaultAsync();

    public async Task<IEnumerable<Order>> GetOrdersByUserName(string name) 
        => await FindByCondition(x => x.UserName.Equals(name)).ToListAsync();

    public async Task<Order> UpdateOrder(Order order)
    {
        await UpdateAsync(order);
        return order;
    }
}
