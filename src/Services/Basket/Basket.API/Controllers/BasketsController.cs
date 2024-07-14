using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories.Interfaces;
using EvenBus.Messages.IntegrationEvent.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketsController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;
    private readonly StockItemGrpcService _stockItemGrpcService;

    public BasketsController(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint, IMapper mapper, StockItemGrpcService stockItemGrpcService)
    {
        _basketRepository = basketRepository;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
        _stockItemGrpcService = stockItemGrpcService;
    }

    [HttpGet("{userName}")]
    [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBasketByUserName([Required] string userName)
    {
        var result =  await _basketRepository.GetBasketByUserName(userName);
        return Ok(result ?? new Cart());
    }

    [HttpPost(Name = "UpdateBaset")]
    [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateBaset([FromBody] Cart cart)
    {
        foreach(var item in cart.Items)
        {
            var stock = await _stockItemGrpcService.GetStock(item.ItemNo);
            item.SetAvailableQuantity(stock.Quantity);
        }

        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddHours(1))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        var result = await _basketRepository.UpdateBasket(cart, options);
        return Ok(result);
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteBasket([Required] string userName)
    {
        var result = await _basketRepository.DeleteBasketFromUserName(userName);
        return Ok(result);
    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout) { 
        var basket = await _basketRepository.GetBasketByUserName(basketCheckout.UserName);

        if (basket == null)
            return NotFound();

        var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
        eventMessage.TotalPrice = basket.TotalPrice;

        await _publishEndpoint.Publish(eventMessage);

        await _basketRepository.DeleteBasketFromUserName(basketCheckout.UserName);

        return Accepted();
    }
}
