using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await unitOfWork.CategoryRepository.GetAllAsync();
                if (categories is null)
                    return BadRequest();
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
                    return BadRequest();
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
                var category = new Category()
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description
                };
                await unitOfWork.CategoryRepository.AddAsync(category);
                return Ok(new { message = "Item has been added sucessfully! " });
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
                var category = new Category()
                {
                    Id = categoryDto.Id,
                    Name = categoryDto.Name,
                    Description = categoryDto.Description
                };
                await unitOfWork.CategoryRepository.UpdateAsync(category);
                return Ok(new { message = "Item has been updated sucessfully! " });
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
                return Ok(new { message = "Item has been deleted sucessfully! " });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
