using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.ToDoDTOs;

public class UpdateToDoDto : ToDoDto
{
    [Required]
    public required int Id { get; set; }
}