using Basket.API.Entities;

namespace Saga.Orchestrator.Services.Interfaces;

public interface ICheckoutSagaService
{
    Task<bool> CheckoutOrder(string userName, BasketCheckoutDto basketCheckout);
}
