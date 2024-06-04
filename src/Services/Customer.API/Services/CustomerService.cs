using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Customer.API.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }


    public async Task<IResult> GetCustomerByUserNameAsync(string userName) => Results.Ok(await _customerRepository.GetCustomerUserNameAsync(userName));

    public async Task<IResult> GetUsers() => Results.Ok(await _customerRepository.GetCustomer());
}
