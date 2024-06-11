using Contract.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Customer.API.Repositories;

public class CustomerRepository : RepositoryBaseAsync<Entities.Customer, int, CustomerContext>, ICustomerRepository
{
    public CustomerRepository(CustomerContext dbContext
        , IUnitOfWork<CustomerContext> uniUnitOfWork) : base(dbContext, uniUnitOfWork)
    {
    }

    public async Task<List<Entities.Customer>> GetCustomer()
    {
        return await FindAllAsync();
    }

    public async Task<Entities.Customer> GetCustomerUserNameAsync(string userName) =>
        await FindByCondition(x => x.UserName.Equals(userName))
        .FirstOrDefaultAsync();

}
