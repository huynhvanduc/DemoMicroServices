using AutoMapper;
using EvenBus.Messages.IntegrationEvent.Events;
using MediatR;
using Ordering.Application.Common.Mapping;
using Ordering.Domain.Entities;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders;

public class CreateOrderCommand : CreateOrUpdateCommand, IRequest<ApiResult<long>>, IMapForm<Order>
{
    public string UserName { get; set; }

    public void Mapping(Profile profile) {
        profile.CreateMap<CreateOrderCommand, Order>();
        profile.CreateMap<BasketCheckoutEvent, CreateOrderCommand>();
    }
}
