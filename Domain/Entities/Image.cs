using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Image
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    public int ToDoId { get; set; }
    public ToDo ToDo { get; set; } = null!;
}