using Contract.Common.Events;

namespace Ordering.Domain.OrderAgggregate.Events;

public class OrderCreatedEvent : BaseEvent
{
    public long Id { get; private set; }

    public string CustomerNo { get; private set; } 

    public string UserName { get; private set; }

    public decimal TotalPrice { get; private set; }


    public string LastName { get; private set; }

    public string EmailAddress { get; private set; }

    public string ShippingAddress { get; private set; }

    public string InvoiceAddress { get; private set; }

    public OrderCreatedEvent(long id, 
        string customerNo, 
        string userName, 
        decimal totalPrice, 
        string emailAddress, 
        string shippingAddress, 
        string invoiceAddress)
    {
        Id = id;
        CustomerNo = customerNo;
        UserName = userName;
        TotalPrice = totalPrice;
        EmailAddress = emailAddress;
        ShippingAddress = shippingAddress;
        InvoiceAddress = invoiceAddress;
    }
}
