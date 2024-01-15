using Domain.Dtos.CategoryDtos;
using Domain.Response;

namespace Infrastructure.Services.CategoryServices;

public interface ICategoryService
{
    public Task<Response<List<GetCategoryDto>>> GetCategories(string? name);
    public Task<Response<GetCategoryDto>> GetCategoryById(int id);
    public Task<Response<int>> AddCategory(AddCategoryDto category);
    public Task<Response<int>> UpdateCategory(UpdateCategoryDto category);
    public Task<Response<bool>> DeleteCategory(int id);
}