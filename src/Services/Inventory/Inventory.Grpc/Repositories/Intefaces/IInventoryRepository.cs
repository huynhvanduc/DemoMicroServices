using Contract.Domain.Interfaces;
using Inventory.Grpc.Entity;

namespace Inventory.Grpc.Repositories.Intefaces;

public interface IInventoryRepository : IMongodbRepositoryBase<InventoryEntry>
{
    Task<int> GetStockQuantity(string itemNo);
}
