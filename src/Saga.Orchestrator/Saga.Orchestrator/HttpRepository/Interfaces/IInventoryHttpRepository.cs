using Shared.DTOs.Inventory;

namespace Saga.Orchestrator.HttpRepository.Interfaces
{
    public interface IInventoryHttpRepository
    {
        Task<string> CreateSalesOrder(SalesProductDto model);

        Task<string> CreateOrderSales(string OrderNo, SalesOrderDto model);

        Task<bool> DeleteOrderByDocumentNo(string documentNo);
    }
}
