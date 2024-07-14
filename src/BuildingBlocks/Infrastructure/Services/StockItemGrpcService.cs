using Inventory.Grpc.Protos;

namespace Basket.API.GrpcServices;

public class StockItemGrpcService 
{
    private readonly StockProtoService.StockProtoServiceClient _stockProtoService;

    public StockItemGrpcService(StockProtoService.StockProtoServiceClient stockProtoService)
    {
        _stockProtoService = stockProtoService ??
            throw new ArgumentNullException(nameof(stockProtoService));
    }

    public async Task<StockModels> GetStock(string itemNo)
    {
        try
        {
            var stockItemRequest = new GetStockRequest { ItemNo = itemNo };
            return _stockProtoService.GetStock(stockItemRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
