using AutoMapper;
using Contract.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders;
using Shared.Services.Emails;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IMessageProducer _messageProducer;

    public OrdersController(IMediator mediator, 
        IOrderRepository orderRepository,
        IMapper mapper, 
        IMessageProducer messageProducer)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _orderRepository = orderRepository;
        _mapper = mapper;
        _messageProducer = messageProducer;
    }

    public static class RouteNames
    {
        public const string GetOrders = nameof(GetOrders);
        public const string CreateOrder = nameof (CreateOrder);
        public const string UpdateOrder = nameof (UpdateOrder);
        public const string DeleteOrder = nameof (DeleteOrder);
    }

    [HttpGet("{name}", Name = RouteNames.GetOrders)]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName([Required] string name)
    {
        var query = new GetOrdersQuery(name);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpGet("test-email")]
    public async Task<IActionResult> TestEmail()
    {
        var message = new MailRequest
        {
            Body = "Hello",
            Subject = "test",
            ToAddress = "vanduc9x98@gmail.com"
        };
        return Ok(message);
    }
}
