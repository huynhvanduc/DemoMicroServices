using AutoMapper;
using Ordering.Application.Common.Mapping;
using Ordering.Domain.Entities;
using Shared.Enums;

namespace Ordering.Application.Common.Models;

public class OrderDto : IMapForm<Order>
{
    public long Id { get; set; }
    public Guid DocumentNo { get; set; }
    public string UserName { get; set; }
    public decimal TotalPrice { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string ShippingAddress { get; set; }
    public string InvoiceAddress { get; set; }
    public Guid CustomerNo { get; set; }
    
    public EOrderStatus Status { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Order, OrderDto>().ReverseMap();
    }
}
