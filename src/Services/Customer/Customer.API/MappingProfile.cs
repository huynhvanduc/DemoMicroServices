using AutoMapper;
using Contract.Common.Interfaces;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ICustomerRepository, CustomerRepository>();
        CreateMap<IUnitOfWork<DbContext>, UnitOfWork<DbContext>>();
        CreateMap<ICustomerService, CustomerService>();
    }
}
