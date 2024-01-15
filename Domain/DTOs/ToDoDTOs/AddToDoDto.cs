using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.ToDoDTOs;

public class AddToDoDto : ToDoDto
{
    [Required] public List<IFormFile> Images { get; set; } = null!;
}