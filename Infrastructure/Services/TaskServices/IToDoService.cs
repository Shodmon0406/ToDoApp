using Domain.Dtos.ToDoDTOs;
using Domain.Filter;
using Domain.Response;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.TaskServices;

public interface IToDoService
{
    Task<Response<List<GetToDoDto>>> GetTask(ToDoFilter filter);
    Task<Response<GetToDoDto>> GetTaskById(int id);
    Task<Response<int>> AddTask(AddToDoDto toDo);
    Task<Response<int>> UpdateTask(UpdateToDoDto toDo);
    Task<Response<string>> AddImage(AddToDoImageDto toDoImages);
    Task<Response<string>> DeleteImage(int imageId);
    Task<Response<bool>> IsCompleted(int id);
    Task<Response<bool>> DeleteTask(int id);
}