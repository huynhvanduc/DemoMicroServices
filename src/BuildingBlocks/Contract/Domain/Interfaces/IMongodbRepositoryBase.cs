using Contract.Domain;
using MongoDB.Driver;

namespace Contract.Domain.Interfaces;

public interface IMongodbRepositoryBase<T> where T : MongoEntity
{
    IMongoCollection<T> FindAll(ReadPreference? readPreference = null);

    Task CreateAsync(T entitty);

    Task UpdateAsync(T entitty);
    Task DeleteAsync(string id);
}
