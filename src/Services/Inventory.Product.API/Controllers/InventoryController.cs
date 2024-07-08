﻿using Inventory.Product.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Inventory;
using Shared.SeedWork;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Inventory.Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    /// <summary>
    /// api/inventory/items/{itemNo}
    /// </summary>
    /// <param name="itemNo"></param>
    /// <returns></returns>
    [Route("item/{itemNo}", Name = "GetAllByItemNo")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllByItemNo([Required] string itemNo)
    {
        var result = await _inventoryService.GetAllByItemNoAsync(itemNo);
        return Ok(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemNo"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    [Route("item/{itemNo}/paging", Name = "GetAllByItemNoPagin")]
    [HttpGet]
    [ProducesResponseType(typeof(PagedList<InventoryEntryDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedList<InventoryEntryDto>>> GetAllByItemNoPagin([Required] string itemNo
        , [FromQuery] GetInventoryPagingQuery query)
    {
        query.ItemNo = itemNo;
        var result = await _inventoryService.GetAllByItemNoPagingAsync(query);
        return Ok(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Route("{id}", Name = "GetInventoryById")]
    [HttpGet]
    [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<InventoryEntryDto>> GetInventoryById([Required] string id)
    {
        var result = await _inventoryService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("purchase/{itemNo}", Name = "PurchaseOrder")]
    [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<InventoryEntryDto>> PurchaseOrder([Required] string itemNo, 
        [FromBody] PurchaseProductDto model)
    {
        var result = await _inventoryService.PurchaseItemAsync(itemNo, model);
        return Ok(result);
    }

    [HttpDelete("{id}", Name = "GetInventoryById")]
    [ProducesResponseType(typeof(InventoryEntryDto), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<InventoryEntryDto>> DeleteById([Required] string id)
    {
        var entity = await _inventoryService.GetByIdAsync(id);
        
        if (entity == null)  return NotFound();

        await _inventoryService.DeleteAsync(id);
        return NoContent();
    }

}