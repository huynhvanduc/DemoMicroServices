﻿using Contract.Common.Interfaces;
using Customer.API.Persistence;

namespace Customer.API.Repositories.Interfaces;

public interface ICustomerRepository : IRepositoryCommandAsync<Entities.Customer, int, CustomerContext>
{
    Task<Entities.Customer> GetCustomerUserNameAsync(string userName);
    Task<List<Entities.Customer>> GetCustomer();
}
