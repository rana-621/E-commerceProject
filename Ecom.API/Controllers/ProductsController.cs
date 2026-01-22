using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTOs;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers;

public class ProductsController : BaseController
{
    private readonly IImageManagementService _imageManagementService;
    public ProductsController(IUnitOfWork unitOfWork, IMapper mapper, IImageManagementService imageManagementService) : base(unitOfWork, mapper)
    {
        _imageManagementService = imageManagementService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllProducts(string? sort, int? categoryId, int pageSize, int pageNumber)
    {
        try
        {
            var products = await unitOfWork.ProductRepository
                .GetAllAsync(sort, categoryId, pageSize, pageNumber);

            return Ok(products);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(id, x => x.Category, x => x.photos);
            var result = mapper.Map<ProductDTO>(product);
            if (product is null)
                return BadRequest(new ResponseAPI(400, "Product not found"));
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> AddProduct(AddProductDTO productDto)
    {
        try
        {
            await unitOfWork.ProductRepository.AddAsync(productDto);
            return Ok(new ResponseAPI(200));
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseAPI(400, ex.Message));
        }

    }

    [HttpPut("update-product")]
    public async Task<IActionResult> UpdateProduct(UpdateProductDTO updateproductDTO)
    {
        try
        {
            await unitOfWork.ProductRepository.UpdateAsync(updateproductDTO);
            return Ok(new ResponseAPI(200));
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseAPI(400, ex.Message));
        }
    }

    [HttpDelete("delete-product/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var product = await unitOfWork.ProductRepository
                .GetByIdAsync(id, m => m.photos, m => m.Category);

            await unitOfWork.ProductRepository.DeleteAsync(product);
            return Ok(new ResponseAPI(200));
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseAPI(400, ex.Message));

        }
    }

}