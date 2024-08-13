using Saga.Orchestrator.Services.Interfaces;

namespace Saga.Orchestrator.Services;

using AutoMapper;
using Basket.API.Entities;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Inventory;
using Shared.DTOs.Orders;
using ILogger = Serilog.ILogger;

public class CheckoutSagaService : ICheckoutSagaService
{
    private readonly IOrderHttpRepository _orderHttpRepository;
    private readonly IBasketHttpRepository _basketHttpRepository;
    private readonly IInventoryHttpRepository _inventoryHttpRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CheckoutSagaService(IOrderHttpRepository orderHttpRepository,
        IBasketHttpRepository basketHttpRepository,
        IInventoryHttpRepository inventoryHttpRepository,
        IMapper mapper,
        ILogger logger)
    {
        _orderHttpRepository = orderHttpRepository;
        _basketHttpRepository = basketHttpRepository;
        _inventoryHttpRepository = inventoryHttpRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<bool> CheckoutOrder(string userName, BasketCheckoutDto basketCheckout)
    {
        // Get cart from BasketHttpRepository
        _logger.Information($"Start: Get cart {userName}");
        var cart = await _basketHttpRepository.GetBasket(userName);
        if (cart == null) return false;
        _logger.Information($"End: Get cart {userName}");

        //Create Order from OrderHttpRepository
        _logger.Information($"Start: Create order {userName}");
        var order = _mapper.Map<CreateOrderDto>(basketCheckout);
        order.TotalPrice = cart.TotalPrice;
        var orderId = await _orderHttpRepository.CreateOrder(order);
        if (orderId < 0) return false;
        var addedOrder = await _orderHttpRepository.GetOrder(orderId);
        _logger.Information($"End: Create order {userName} with DocumentNo-{addedOrder.DocumentNo}");
        //Get Order by order id

        var inventoryDocumentNo = new List<string>();
        bool result;
        try
        {
            //Sales Items from☻8 InventoryRepository
            foreach (var item in cart.Items)
            {
                _logger.Information($"Start: Sale Item No: {item.ItemNo} - Quantity {item.Quantity}");
                var saleOrder = new SalesProductDto(addedOrder.DocumentNo.ToString(), item.Quantity);
                saleOrder.SetItemNo(item.ItemNo);
                var documentNo = await _inventoryHttpRepository.CreateSalesOrder(saleOrder);
                inventoryDocumentNo.Add(documentNo);
                _logger.Information($"End: Sale Item No: {item.ItemNo} - Quantity {item.Quantity}");
            }
            result = await _basketHttpRepository.DeleteBasket(userName);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            RollbackCheckoutOrder(userName, addedOrder.Id, inventoryDocumentNo);
            result = false;
        }


        return result;
    }

    private async Task RollbackCheckoutOrder(string userName, long orderId, List<string> inventoryDocumentNos)
    {
        _logger.Information($"Start: RollbackCheckoutOrder for username: {userName}, "
            + $"order id = {orderId}, "
            + $"inventory document no: {String.Join(", ", inventoryDocumentNos)}");

        var deletedDocumentNo = new List<string>();
        _logger.Information($"Start: Delete Order Id: {orderId}");
        await _orderHttpRepository.DeleteOrder(orderId);
        _logger.Information($"End: Delete Order Id: {orderId}");

        //Delete order by order's id, order's document no
        foreach(var documentNo in inventoryDocumentNos)
        {
            await _inventoryHttpRepository.DeleteOrderByDocumentNo(documentNo);
            deletedDocumentNo.Add(documentNo);
        }

        _logger.Information($"End Deleted document no: {String.Join(", ", inventoryDocumentNos)}");

    }
}
