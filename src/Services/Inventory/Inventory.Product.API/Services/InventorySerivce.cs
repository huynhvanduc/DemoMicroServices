using AutoMapper;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Inventory.Product.API.Entities;
using Inventory.Product.API.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Configurations;
using Shared.DTOs.Inventory;
using Shared.SeedWork;

namespace Inventory.Product.API.Services;

public class InventorySerivce : MongoDbRepository<InventoryEntry>, IInventoryService
{
    private readonly IMapper _mapper;

    public InventorySerivce(IMongoClient client, MongDbSettings settings, IMapper mapper)
        : base(client, settings)
    {
        _mapper = mapper;
    }

    public async Task DeleteByDocumentNoAsync(string documentNo)
    {
        FilterDefinition<InventoryEntry> filter = Builders<InventoryEntry>.Filter.Eq(s => s.DocumentNo, documentNo);
        await Collection.DeleteManyAsync(filter);
    }

    public async Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo)
    {
        var entities = await FindAll()?
            .Find(x => x.ItemNo.Equals(itemNo))
            .ToListAsync();

        var result = _mapper.Map<IEnumerable<InventoryEntryDto>>(entities);

        return result;
    }

    public async Task<PagedList<InventoryEntryDto>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query)
    {
        var filterSearchItem = Builders<InventoryEntry>.Filter.Empty;
        var filterItemNo = Builders<InventoryEntry>.Filter.Eq(x => x.ItemNo, query.ItemNo);

        if (!string.IsNullOrEmpty(query.SearchTerm))
        {
            filterSearchItem = Builders<InventoryEntry>.Filter.Eq(x => x.DocumentNo, query.SearchTerm);
        }

        var andFilter = filterItemNo & filterSearchItem;

        var pagedList = await Collection.PaginatedListAsync(andFilter, query.PageIndex, query.PageSize);

        var items = _mapper.Map<IEnumerable<InventoryEntryDto>>(pagedList);

        var result = new PagedList<InventoryEntryDto>(items, pagedList.GetMetaData().TotalItems, query.PageIndex, query.PageSize);

        return result;
    }

    public async Task<InventoryEntryDto> GetByIdAsync(string id)
    {
        FilterDefinition<InventoryEntry> filter = Builders<InventoryEntry>.Filter.Eq(x => x.Id, id);
        var entity = await FindAll().Find(filter).FirstOrDefaultAsync();
        var result = _mapper.Map<InventoryEntryDto>(entity);
        return result;
    }

    public async Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model)
    {
        var entity = new InventoryEntry(ObjectId.GenerateNewId().ToString())
        {
            ItemNo = itemNo,
            Quantity = model.Quantity,
            DocumentType = model.DocumentType,
        };

        await CreateAsync(entity);
        var result = _mapper.Map<InventoryEntryDto>(entity);

        return result;
    }

    public async Task<InventoryEntryDto> SalesItemAsync(string itemNo, SalesProductDto model)
    {
        var itemToAdd = new InventoryEntry(ObjectId.GenerateNewId().ToString())
        {
            ItemNo = itemNo,
            ExternalDocumentNo = model.ExternalDocumentNo,
            Quantity = model.Quantity * -1,
            DocumentType = model.DocumentType
        };

        await CreateAsync(itemToAdd);
        var result = _mapper.Map<InventoryEntryDto>(itemToAdd);
        return result;
    }

    public async Task<string> SalesOrderAsync(SalesOrderDto model)
    {
        var documentNo = Guid.NewGuid().ToString();
        foreach(var saleItem in model.SaleItems)
        {
            var itemToAdd = new InventoryEntry(ObjectId.GenerateNewId().ToString())
            {
                DocumentNo = documentNo,
                ItemNo = saleItem.ItemNo,
                ExternalDocumentNo = model.OrderNo,
                Quantity = saleItem.Quantity * -1,
                DocumentType = saleItem.DocumentType
            };

            await CreateAsync(itemToAdd);
        }
        return documentNo;
    }
}
