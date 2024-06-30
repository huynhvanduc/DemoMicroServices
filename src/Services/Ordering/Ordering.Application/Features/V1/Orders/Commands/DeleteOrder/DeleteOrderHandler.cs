using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Exeptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using ILogger = Serilog.ILogger;

namespace Ordering.Application.Features.V1.Orders.Commands;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger _logger;

    public DeleteOrderHandler(IOrderRepository orderRepository, ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
        if (orderEntity is null)
            throw new NotFoundException(nameof(Order), request.Id);

        await _orderRepository.DeleteAsync(orderEntity);
        await _orderRepository.SaveChangesAsync();

        _logger.Information($"Order {orderEntity.Id} was deleted.");

        return Unit.Value;
    }
}
