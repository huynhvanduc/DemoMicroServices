using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using ILogger = Serilog.ILogger;


namespace Ordering.Application.Features.V1.Orders.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly ILogger _logger;

    private const string METHOD_NAME = "CreateOrderCommandHandler";

    public CreateOrderCommandHandler(IMapper mapper, 
        IOrderRepository repository,
        ILogger logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: {METHOD_NAME} - UserName: {request.UserName}");
        Order order = _mapper.Map<Order>(request);
        _repository.Create(order);
        order.AddedOrder();
        await _repository.SaveChangesAsync();
        _logger.Information($"Order: {order.Id} is successfully created");
        return new ApiSuccessResult<long>(order.Id);
    }
}
