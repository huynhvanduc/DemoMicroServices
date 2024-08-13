using Basket.API.Entities;
using Contract.Sagas.OrderManager;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.OrderManager;
using Saga.Orchestrator.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Saga.Orchestrator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ISagaOrderManager<BasketCheckoutDto, OrderResponse> _sagaOrderManager;

        public CheckoutController(ISagaOrderManager<BasketCheckoutDto, OrderResponse> sagaOrderManager)
        {
            _sagaOrderManager = sagaOrderManager;
        }

        [HttpPost]
        [Route("{username}")]
        public OrderResponse CheckoutOrder([Required] string userName, [FromBody] BasketCheckoutDto model)
        {
            model.UserName = userName;
            var result = _sagaOrderManager.CreateOrder(model);
            return result;
        }
    }
}
