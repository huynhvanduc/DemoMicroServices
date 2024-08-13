using AutoMapper;
using Basket.API.Entities;
using Shared.DTOs.Basket;
using Shared.DTOs.Inventory;
using Shared.DTOs.Orders;

namespace Saga.Orchestrator;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BasketCheckoutDto, CreateOrderDto>();
        CreateMap<CartItemDto, SaleItemDto>();
    }
}
