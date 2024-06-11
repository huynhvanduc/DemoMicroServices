using Ordering.Application.Common.Mapping;
using Ordering.Domain.Entities;

namespace Ordering.Application.Common.Models;

public class OrderDto : IMapForm<Order>
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public decimal TotalPrice { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string ShippinfAddress { get; set; }
    public string InvoiceAddress { get; set; }
    public string Status { get; set; }  
}
