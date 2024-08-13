using Contract.Common.Events;
using Ordering.Domain.OrderAgggregate.Events;
using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Domain.Entities;

public class Order : AuditableEventEntity<long>
{
    public Guid DocumentNo { get; set; } = Guid.NewGuid();

    [Required]
    public string UserName { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; }

    public Guid CustomerNo { get; set; } = Guid.NewGuid();

    [Required]
    [EmailAddress]
    [Column(TypeName = "nvarchar(250)")]
    public string EmailAddress { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string ShippingAddress {get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string InvoiceAddress {get; set; }

    public EOrderStatus Status { get; set; }

    public Order AddedOrder()
    {
        AddDomainEvent(
            new OrderCreatedEvent(
                Id,
                CustomerNo.ToString(),
                UserName, 
                TotalPrice,
                EmailAddress, 
                ShippingAddress, 
                InvoiceAddress));
        return this;
    }

    public Order DeletedOrder()
    {
        AddDomainEvent(new OrderDeleteEvent(Id));
        return this;
    }
}
