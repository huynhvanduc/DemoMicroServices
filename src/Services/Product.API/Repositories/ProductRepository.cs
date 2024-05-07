using Contract.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;
using Product.API.Repositories.Interfaces;

namespace Product.API.Repositories;

public class ProductRepository : RepositoryCommandAsync<CatalogProduct, long, ProductContext>, IProductRepository
{
    public ProductRepository(ProductContext dbContext, IUnitOfWork<ProductContext> uniUnitOfWork): base(dbContext, uniUnitOfWork)
    {
    }

    public Task CreateProduct(CatalogProduct product) => CreateAsync(product);

    public async Task DeleteProduct(long id)
    {
        var product = await GetProduct(id);
        if (product is not null)
            DeleteAsync(product);
    }

    public async Task<CatalogProduct> GetProduct(long id) => await GetByIdAsync(id);

    public async Task<CatalogProduct> GetProductByNo(string ProductNo)
    {
        return await FindByCondition(x => x.No.Equals(ProductNo)).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<CatalogProduct>> GetProducts() => await FindAll().ToListAsync();

    public async Task UpdateProduct(CatalogProduct product) => await UpdateAsync(product);
}
