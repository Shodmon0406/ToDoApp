using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.ImageDTOs;

public class ImageDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string ImageName { get; set; } = null!;
}