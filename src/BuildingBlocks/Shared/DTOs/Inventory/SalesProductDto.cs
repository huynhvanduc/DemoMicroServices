﻿using Shared.Enums;

namespace Shared.DTOs.Inventory;

public record SalesProductDto(string ExternalDocumentNo, int Quantity) 
{
    public EDocumentType DocumentType { get; set; } = EDocumentType.Sale;
    public string ItemNo { get; set; }
    public void SetItemNo(string itemNo)
    {
        ItemNo = itemNo;
    }
}

