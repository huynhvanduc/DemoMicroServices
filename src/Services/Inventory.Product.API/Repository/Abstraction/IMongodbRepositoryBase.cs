using Inventory.Product.API.Entities.Abstraction;
using MongoDB.Driver;

namespace Inventory.Product.API.Repository.Abstraction;

public interface IMongodbRepositoryBase<T> where T: MongoEntity
{
    IMongoCollection<T> FindAll(ReadPreference? readPreference = null);

    Task CreateAsync(T entitty);

    Task UpdateAsync(T entitty);
    Task DeleteAsync(string id);    
}
