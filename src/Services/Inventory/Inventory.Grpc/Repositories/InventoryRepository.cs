using Inventory.Grpc.Entity;
using Inventory.Grpc.Repositories.Intefaces;
using Infrastructure.Common;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Grpc.Repositories;

public class InventoryRepository : MongoDbRepository<InventoryEntry>, IInventoryRepository
{
    public InventoryRepository(IMongoClient client, MongDbSettings settings) : base(client, settings)
    {
    }

    public async Task<int> GetStockQuantity(string itemNo)
    {
        return Collection.AsQueryable()
        .Where(x => x.ItemNo.Equals(itemNo))
        .Sum(x => x.Quantity);
    }
}
