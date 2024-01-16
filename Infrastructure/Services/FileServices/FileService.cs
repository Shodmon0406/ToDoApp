using System.Net;
using Domain.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.FileServices;

public class FileService(
    IWebHostEnvironment hostEnvironment,
    ILogger<FileService> logger) : IFileService
{
    public async Task<Response<string>> CreateFile(IFormFile file)
    {
        try
        {
            logger.LogInformation("Create file service started at: {DateTime}", DateTime.UtcNow);
            const string directory = "images";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory($"{hostEnvironment.WebRootPath}/{directory}");
            }

            var fileName = string.Format($"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");
            var fullPath = Path.Combine(hostEnvironment.WebRootPath, directory, fileName);
            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            logger.LogInformation("Create file service successfully finished at {DateTime}", DateTime.UtcNow);
            return new Response<string>(fileName);
        }
        catch (Exception e)
        {
            logger.LogError("Error in the create file service: {Message}", e.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateFile(IFormFile newFile, string oldFile)
    {
        try
        {
            await CreateFile(newFile);
            DeleteFile(oldFile);
            return new Response<string>("File successfully updated.");
        }
        catch (Exception e)
        {
            logger.LogError("Error in the update file service: {Message}", e.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public Response<string> DeleteFile(string fileName)
    {
        try
        {
            var fullPath = Path.Combine(hostEnvironment.WebRootPath, "images", fileName);
            File.Delete(fullPath);
            return new Response<string>("File successfully deleted.");
        }
        catch (Exception e)
        {
            logger.LogError("Error in the delete service: {Message}", e.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}