using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers;


public class CategoriesController : BaseController
{
    public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var categories = await unitOfWork.CategoryRepository.GetAllAsync();
            if (categories is null)
                return BadRequest(new ResponseAPI(400));
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        try
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return BadRequest(new ResponseAPI(400, $"Not Found Category id = {id}"));
            return Ok(category);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("add-category")]
    public async Task<IActionResult> AddCategory(AddCategoryDTO categoryDto)
    {
        try
        {
            var category = mapper.Map<Category>(categoryDto);
            await unitOfWork.CategoryRepository.AddAsync(category);
            return Ok(new ResponseAPI(200, "Item has been added !"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("update-category/{id}")]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO categoryDto)
    {
        try
        {
            var category = mapper.Map<Category>(categoryDto);
            await unitOfWork.CategoryRepository.UpdateAsync(category);
            return Ok(new ResponseAPI(200, "Item has been updated !"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("delete-category/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            await unitOfWork.CategoryRepository.DeleteAsync(id);
            return Ok(new ResponseAPI(200, "Item has been deleted !"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
