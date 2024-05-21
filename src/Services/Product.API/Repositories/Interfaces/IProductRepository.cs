﻿using Contract.Common.Interfaces;
using Product.API.Entities;
using Product.API.Persistence;

namespace Product.API.Repositories.Interfaces;

public interface IProductRepository : IRepositoryCommandAsync<CatalogProduct, long, ProductContext>
{
    Task<IEnumerable<CatalogProduct>> GetProducts();
    Task<CatalogProduct> GetProduct(long id);
    Task<CatalogProduct> GetProductByNo(string ProductNo);
    Task CreateProduct(CatalogProduct product);
    Task UpdateProduct(CatalogProduct product);
    Task DeleteProduct(long id);
}