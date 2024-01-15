using System.Net;
using Domain.Dtos.CategoryDtos;
using Domain.Response;
using Infrastructure.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class CategoryController(ICategoryService service) : BaseController
{
    [HttpGet("get-categories")]
    public async Task<IActionResult> GetTasks(string? name)
    {
        var result = await service.GetCategories(name);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-category-by-id")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await service.GetCategoryById(id);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<GetCategoryDto>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("add-category")]
    public async Task<IActionResult> AddTask([FromBody]AddCategoryDto category)
    {
        if (ModelState.IsValid)
        {
            var result = await service.AddCategory(category);
            return StatusCode(result.StatusCode, result);
        }

        return StatusCode(400, "Error");
    }

    [HttpPut("update-category")]
    public async Task<IActionResult> UpdateCategory([FromBody]UpdateCategoryDto updateCategory)
    {
        if (ModelState.IsValid)
        {
            var result = await service.UpdateCategory(updateCategory);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-category")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await service.DeleteCategory(id);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}