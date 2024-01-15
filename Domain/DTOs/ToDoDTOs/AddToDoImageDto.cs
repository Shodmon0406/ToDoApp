using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.ToDoDTOs;

public class AddToDoImageDto
{
    [Required]
    public int ToDoId { get; set; }
    [Required]
    public List<IFormFile> Images { get; set; } = null!;
}