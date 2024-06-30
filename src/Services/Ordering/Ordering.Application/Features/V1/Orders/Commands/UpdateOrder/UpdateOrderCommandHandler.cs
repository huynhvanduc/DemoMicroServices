using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Exeptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using ILogger = Serilog.ILogger;

namespace Ordering.Application.Features.V1.Orders.Commands;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    private const string MethodName = nameof(UpdateOrderCommandHandler);

    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
        if (orderEntity is null)
            throw new NotFoundException(nameof(Order), request.Id);

        _logger.Information($"BEGIN: {MethodName} - Order: {request.Id}");

        orderEntity = _mapper.Map(request, orderEntity);
        var updateOrder = await _orderRepository.UpdateOrder(orderEntity);
        await _orderRepository.SaveChangesAsync();

        _logger.Information($"Order {request.Id} was updated");

        var result = _mapper.Map<OrderDto>(updateOrder);

        _logger.Information($"END: {MethodName} - Order: {request.Id}");
        return new ApiSuccessResult<OrderDto>(result);
    }
}
