using System.Net;
using Domain.Dtos.CategoryDtos;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.CategoryServices;

public class CategoryService(DataContext context) : ICategoryService
{
    public async Task<Response<List<GetCategoryDto>>> GetCategories(string? name)
    {
        try
        {
            var result = await context.Categories.Select(c => new GetCategoryDto()
            {
                Id = c.Id,
                Name = c.Name,
            }).ToListAsync();
            return new Response<List<GetCategoryDto>>(result);
        }
        catch (Exception e)
        {
            return new Response<List<GetCategoryDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetCategoryDto>> GetCategoryById(int id)
    {
        try
        {
            var result = await context.Categories.Select(c => new GetCategoryDto()
            {
                Id = c.Id,
                Name = c.Name,
            }).FirstOrDefaultAsync(c => c.Id == id);
            return new Response<GetCategoryDto>(result);
        }
        catch (Exception e)
        {
            return new Response<GetCategoryDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> AddCategory(AddCategoryDto category)
    {
        try
        {
            var mapped = new Category()
            {
                Name = category.Name,
            };
            await context.Categories.AddAsync(mapped);
            await context.SaveChangesAsync();
            return new Response<int>(mapped.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> UpdateCategory(UpdateCategoryDto category)
    {
        try
        {
            var oldCategory = await context.Categories.FindAsync(category.Id);
            if (oldCategory == null) return new Response<int>(HttpStatusCode.NotFound, "Category not found!");
            oldCategory.Name = category.Name;
            await context.SaveChangesAsync();
            return new Response<int>(oldCategory.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteCategory(int id)
    {
        try
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null) return new Response<bool>(HttpStatusCode.NotFound, "Category not found!");
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}