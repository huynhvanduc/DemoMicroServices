using Contract.Domain.Interfaces;
using Inventory.Product.API.Entities;
using Shared.DTOs.Inventory;
using Shared.SeedWork;

namespace Inventory.Product.API.Services.Interfaces;

public interface IInventoryService : IMongodbRepositoryBase<InventoryEntry>
{
    Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo);

    Task<PagedList<InventoryEntryDto>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query);

    Task<InventoryEntryDto> GetByIdAsync(string id);

    Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model);
}
