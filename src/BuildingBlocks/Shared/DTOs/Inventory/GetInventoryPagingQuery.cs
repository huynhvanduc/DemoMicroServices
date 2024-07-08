using Shared.SeedWork;

namespace Shared.DTOs.Inventory;

public class GetInventoryPagingQuery : PagingRequestParameters
{
    private string _itemNo;

    public string? ItemNo
    {
        get
        {
            return _itemNo;
        }
        set
        {
            _itemNo = value;
        }
    }

    public string? SearchTerm { get; set; }
}
