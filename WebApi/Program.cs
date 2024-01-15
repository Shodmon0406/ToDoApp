using Infrastructure.Data;
using Infrastructure.Services.CategoryServices;
using Infrastructure.Services.FileServices;
using Infrastructure.Services.TaskServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(configure 
    => configure.UseNpgsql(connectionString));

builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

app.UseCors(
    corsPolicyBuilder => corsPolicyBuilder.WithOrigins("http://localhost:3000", "http://localhost:5173")
        .SetIsOriginAllowed(_ => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
);

try
{
    var serviceProvider = app.Services.CreateScope().ServiceProvider;
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();
}
catch (Exception)
{
    //
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseStaticFiles();

app.MapControllers();


app.Run();