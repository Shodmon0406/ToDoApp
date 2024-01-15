using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ToDo
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [MaxLength(1000)]
    public string Description { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public List<Image> Images { get; set; } = null!;
}