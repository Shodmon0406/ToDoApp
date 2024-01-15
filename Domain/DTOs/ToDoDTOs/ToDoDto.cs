using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.ToDoDTOs;

public class ToDoDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;
    [Required, MaxLength(1000)]
    public string Description { get; set; } = null!;
}