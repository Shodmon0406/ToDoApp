using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ToDo> Tasks { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;
}