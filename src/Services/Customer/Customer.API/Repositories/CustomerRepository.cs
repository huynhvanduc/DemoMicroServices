using Contract.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
namespace Customer.API.Repositories;

public class CustomerRepository : RepositoryBaseAsync<Entities.Customer, int, CustomerContext>, ICustomerRepository
{
    public CustomerRepository(CustomerContext dbContext
        , IUnitOfWork<CustomerContext> uniUnitOfWork) : base(dbContext, uniUnitOfWork)
    {
    }

    public async Task<List<Entities.Customer>> GetCustomer()
    {
        var result = await FindAllAsync();
        return result;
    }

    public async Task<Entities.Customer> GetCustomerUserNameAsync(string userName) =>
        await FindByCondition(x => x.UserName.Equals(userName))
        .FirstOrDefaultAsync();

}
