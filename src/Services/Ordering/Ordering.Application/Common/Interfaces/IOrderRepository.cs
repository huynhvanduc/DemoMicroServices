using Contract.Common.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Common.Interfaces;

public interface IOrderRepository  : IRepositoryBaseAsync<Order, long>
{
    Task<IEnumerable<Order>> GetOrdersByUserName(string name);

    Task<Order> CreateOrder(Order order);

    Task<Order> UpdateOrder(Order order);

    void DeleteOrder(Order order);
    Task<Order> GetOrdersByDocumentNo(string documentNo);
}
