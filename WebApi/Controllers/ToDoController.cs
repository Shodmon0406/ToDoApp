using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Dtos.ToDoDTOs;
using Domain.Filter;
using Domain.Response;
using Infrastructure.Services.TaskServices;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ToDoController(IToDoService service) : BaseController
{
    [HttpGet("get-to-dos")]
    public async Task<IActionResult> GetToDos(ToDoFilter filter)
    {
        var result = await service.GetTask(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-to-do-by-id")]
    public async Task<IActionResult> GetToDo([Required]int id)
    {
        if (ModelState.IsValid)
        {
            var result = await service.GetTaskById(id);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<GetToDoDto>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("add-to-do")]
    public async Task<IActionResult> AddTodo([FromForm]AddToDoDto addToDo)
    {
        if (ModelState.IsValid)
        {
            var result = await service.AddTask(addToDo);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("update-to-do")]
    public async Task<IActionResult> UpdateToDo([FromBody]UpdateToDoDto updateToDo)
    {
        if (ModelState.IsValid)
        {
            var result = await service.UpdateTask(updateToDo);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("add-to-do-images")]
    public async Task<IActionResult> AddToDoImages([FromForm]AddToDoImageDto toDoImages)
    {
        if (ModelState.IsValid)
        {
            var result = await service.AddImage(toDoImages);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<string>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-to-do-image")]
    public async Task<IActionResult> DeleteToDoImage([Required]int imageId)
    {
        if (ModelState.IsValid)
        {
            var result = await service.DeleteImage(imageId);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<string>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("is-completed")]
    public async Task<IActionResult> IsCompleted([Required]int id)
    {
        if (ModelState.IsValid)
        {
            var result = await service.IsCompleted(id);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpDelete("delete-to-do")]
    public async Task<IActionResult> DeleteToDo([Required]int id)
    {
        if (ModelState.IsValid)
        {
            var result = await service.DeleteTask(id);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}