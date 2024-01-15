using System.ComponentModel.DataAnnotations;
using Domain.DTOs.ImageDTOs;

namespace Domain.Dtos.ToDoDTOs;

public class GetToDoDto : ToDoDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public bool IsCompleted { get; set; }
    public List<ImageDto> Images { get; set; } = null!;
}