using System.Net;
using Domain.DTOs.ImageDTOs;
using Domain.Dtos.ToDoDTOs;
using Domain.Entities;
using Domain.Filter;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Services.FileServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.TaskServices;

public class ToDoService(
    DataContext context,
    IServiceProvider serviceProvider,
    IFileService fileService,
    ILogger<ToDoService> logger) : IToDoService
{
    public async Task<Response<List<GetToDoDto>>> GetTask(ToDoFilter filter)
    {
        try
        {
            await using var service = serviceProvider.CreateAsyncScope();
            var dbContext = service.ServiceProvider.GetRequiredService<DataContext>();

            var toDos = context.Tasks.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(filter.ToDoName))
                toDos = toDos.Where(toDo => toDo.Name.ToLower().Contains(filter.ToDoName.ToLower()));
            var result = await (from toDo in toDos 
                select new GetToDoDto()
            {
                Id = toDo.Id,
                Name = toDo.Name,
                Description = toDo.Description,
                IsCompleted = toDo.IsCompleted,
                Images = toDo.Images.Select(image => new ImageDto()
                {
                    Id = image.Id, ImageName = image.Name
                }).ToList()
            }).AsNoTracking().ToListAsync();
            return new Response<List<GetToDoDto>>(result);
        }
        catch (Exception e)
        {
            return new Response<List<GetToDoDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetToDoDto>> GetTaskById(int id)
    {
        try
        {
            var result = await context.Tasks.Where(t => t.Id == id).Select(t => new GetToDoDto()
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                Images = t.Images.Select(x => new ImageDto()
                {
                    Id = x.Id, ImageName = x.Name
                }).ToList()
            }).FirstOrDefaultAsync();
            return new Response<GetToDoDto>(result);
        }
        catch (Exception e)
        {
            return new Response<GetToDoDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> AddTask(AddToDoDto toDo)
    {
        try
        {
            var mapped = new ToDo()
            {
                Name = toDo.Name,
                IsCompleted = false,
                Description = toDo.Description
            };
            await context.Tasks.AddAsync(mapped);
            await context.SaveChangesAsync();

            var images = new List<Image>();
            foreach (var image in toDo.Images)
            {
                var imageName = await fileService.CreateFile(image);
                var newImage = new Image()
                {
                    Name = imageName.Data ?? "Hello world",
                    ToDoId = mapped.Id
                };
                images.Add(newImage);
            }

            await context.Images.AddRangeAsync(images);
            await context.SaveChangesAsync();
            return new Response<int>(mapped.Id);
        }
        catch (Exception e)
        {
            logger.LogError("Error in the add to do service: {Message}", e.Message);
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> UpdateTask(UpdateToDoDto toDo)
    {
        try
        {
            var oldToDo = await context.Tasks.FindAsync(toDo.Id);
            if (oldToDo == null) return new Response<int>(HttpStatusCode.NotFound, "ToDo not found!");
            oldToDo.Name = toDo.Name;
            oldToDo.Description = toDo.Description;
            oldToDo.IsCompleted = oldToDo.IsCompleted;
            await context.SaveChangesAsync();
            return new Response<int>(oldToDo.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> AddImage(AddToDoImageDto toDoImages)
    {
        try
        {
            var existToDo = await context.Tasks.Where(x => x.Id == toDoImages.ToDoId).AsNoTracking()
                .FirstOrDefaultAsync();
            if (existToDo == null) return new Response<string>(HttpStatusCode.NotFound, "ToDo not found!");
            var images = new List<Image>();
            foreach (var image in toDoImages.Images)
            {
                var imageName = await fileService.CreateFile(image);
                var newImage = new Image()
                {
                    Name = imageName.Data!,
                    ToDoId = existToDo.Id
                };
                images.Add(newImage);
            }

            await context.Images.AddRangeAsync(images);
            await context.SaveChangesAsync();
            return new Response<string>("Images successfully updated.");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> DeleteImage(int imageId)
    {
        try
        {
            var existToDoImage = await context.Images.Where(x => x.Id == imageId).AsNoTracking().FirstOrDefaultAsync();
            if (existToDoImage == null) return new Response<string>(HttpStatusCode.NotFound, "ToDo not found!");
            fileService.DeleteFile(existToDoImage.Name);
            return new Response<string>("Image successfully deleted.");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> IsCompleted(int id)
    {
        try
        {
            var result = await context.Tasks.FindAsync(id);
            if (result == null!) return new Response<bool>(HttpStatusCode.BadRequest, "ToDo not found!");
            result.IsCompleted = !result.IsCompleted;
            await context.SaveChangesAsync();
            return new Response<bool>(result.IsCompleted);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteTask(int id)
    {
        try
        {
            var task = await context.Tasks.Where(toDo => toDo.Id == id).Select(toDo => new { toDo, toDo.Images })
                .FirstOrDefaultAsync();
            if (task == null) return new Response<bool>(HttpStatusCode.NotFound, "ToDo not found!");
            context.Tasks.Remove(task.toDo);
            await context.SaveChangesAsync();
            task.Images.ForEach(image => fileService.DeleteFile(image.Name));
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}