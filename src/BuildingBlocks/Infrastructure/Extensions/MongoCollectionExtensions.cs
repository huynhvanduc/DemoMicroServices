using MongoDB.Driver;
using Shared.SeedWork;

namespace Infrastructure.Extensions;

public static class MongoCollectionExtensions
{
    public static Task<PagedList<TDes>> PaginatedListAsync<TDes>(
        this IMongoCollection<TDes> collection,
        FilterDefinition<TDes> filter,
        int pageIndex,
        int pageNumber)
        where TDes : class
    => PagedList<TDes>.ToPagedList(collection, filter, pageIndex, pageNumber);
}
