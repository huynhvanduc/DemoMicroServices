﻿using MongoDB.Driver;

namespace Shared.SeedWork;

public class PagedList<T> : List<T>
{
    private MetaData _metaData { get; set; }

    public MetaData GetMetaData()
    {
        return _metaData;
    }

    public PagedList(IEnumerable<T> items, long totalItems, int pageNumber, int pageSize)
    {
        _metaData = new MetaData()
        {
            TotalItems = totalItems,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            //TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
        AddRange(items);
    }

    public static async Task<PagedList<T>> ToPagedList(
        IMongoCollection<T> source, 
        FilterDefinition<T> filter, 
        int pageNumber, 
        int pageSize)
    {
        var count = await source.Find(filter).CountDocumentsAsync();

        var items = await source.Find(filter)
                                .Skip((pageNumber - 1) * pageSize)
                                .Limit(pageSize)
                                .ToListAsync();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}