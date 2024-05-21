using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.API.Entities;
using Product.API.Repositories.Interfaces;
using Shared.DTOs.Product;
using System.ComponentModel.DataAnnotations;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductsController(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var products = await _repository.GetProducts();

        var result = products is null ? null : _mapper.Map<IEnumerable<ProductDto>>(products);
        return Ok(result);
    }

    [HttpGet("product/{id:long}")]
    public async Task<IActionResult> Get([Required] long id)
    {
        var product = await _repository.GetProduct(id);
        if (product == null)
            return NotFound();

        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
    {
        var product = _mapper.Map<CatalogProduct>(productDto);
        await _repository.CreateProduct(product);
        await _repository.SaveChangesAsync();
        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateProduct([Required] long id, [FromBody] UpdateProductDto productDto)
    {
        var product = await _repository.GetProduct(id);

        if (product == null)
            return NotFound();

        var updateProduct = _mapper.Map(productDto, product);
        await _repository.UpdateAsync(updateProduct);
        await _repository.SaveChangesAsync();
        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteProduct([Required] long id)
    {
        var product = await _repository.GetProduct(id);

        if (product == null)
            return NotFound();

        await _repository.DeleteAsync(product);
        await _repository.SaveChangesAsync();

        return Ok(product);
    }

    [HttpGet("product/{productNo}")]
    public async Task<IActionResult> GetProductByNo([Required] string productNo)
    {
        var product = await _repository.GetProductByNo(productNo);

        if (product == null)
            return NotFound();

        var result = product is null ? null : _mapper.Map<ProductDto>(product);
        return Ok(result);
    }
}

