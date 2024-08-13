using AutoMapper;
using Contract.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.V1.Orders;
using Ordering.Application.Features.V1.Orders.Commands;
using Ordering.Application.Features.V1.Orders.Commands.DeleteByDocumentNo;
using Ordering.Application.Features.V1.Orders.Queries.GetOrderById;
using Shared.DTOs.Orders;
using Shared.SeedWork;
using Shared.Services.Emails;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISmtpEmailService _smtpEmailService;
    private readonly IMapper _mappper;

    public OrdersController(IMediator mediator,
        ISmtpEmailService smtpEmailService,
        IMapper mappper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _smtpEmailService = smtpEmailService;
        _mappper = mappper;
    }

    public static class RouteNames
    {
        public const string GetOrders = nameof(GetOrders);
        public const string GetOrder = nameof(GetOrder);
        public const string CreateOrder = nameof (CreateOrder);
        public const string UpdateOrder = nameof (UpdateOrder);
        public const string DeleteOrder = nameof (DeleteOrder);
        public const string DeleteByDocumentNo = nameof(DeleteByDocumentNo);
    }

    [HttpGet("{name}", Name = RouteNames.GetOrders)]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName([Required] string name)
    {

        var query = new GetOrdersQuery(name);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost(Name = RouteNames.CreateOrder)]
    [ProducesResponseType(typeof(ApiResult<long>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody]CreateOrderDto model)
    {
        var command = _mappper.Map<CreateOrderCommand>(model);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id:long}", Name = RouteNames.GetOrder)]
    public async Task<ActionResult<OrderDto>> GetOrder([Required] long id)
    {
        var query = new GetOrderByIdQuery(id);
        var request = await _mediator.Send(query);
        return Ok(request);

    }

    [HttpDelete("{id:long}", Name = RouteNames.DeleteOrder)]
    public async Task<ActionResult> DeleteOrder([Required] long id)
    {
        var command = new DeleteOrderCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("document-no/{documentNo}", Name = RouteNames.DeleteByDocumentNo)]
    public async Task<ApiResult<bool>> DeleteOrderByDocumentNo([Required]string documentNo)
    {
        var command = new DeleteOrderByDocumentNoCommand(documentNo);
        var result = await _mediator.Send(command);
        return result;
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

        _smtpEmailService.SendEmailAsync(message);
        return Ok(message);
    }
}
