using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;

    public CreateOrderCommandHandler(IMapper mapper, IOrderRepository repository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = _mapper.Map<Order>(request);
         _repository.CreateAsync(order);

        var result = await _repository.SaveChangesAsync();

        return new ApiSuccessResult<long>(result);
    }
}
