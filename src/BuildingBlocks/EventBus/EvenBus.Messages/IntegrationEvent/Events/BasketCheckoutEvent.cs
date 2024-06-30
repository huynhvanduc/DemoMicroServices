﻿using EvenBus.Messages.IntegrationEvent.Interfaces;

namespace EvenBus.Messages.IntegrationEvent.Events;

public record BasketCheckoutEvent : IntegrationBaseEvent, IBasketCheckoutEvent
{
    public string UserName { get; set; }
    public decimal TotalPrice { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string InvoiceAddress { get; set; }
    public string ShippingAddress { get; set; }
}