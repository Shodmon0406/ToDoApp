using Domain.Response;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.FileServices;

public interface IFileService
{
    Task<Response<string>> CreateFile(IFormFile file);
    Task<Response<string>> UpdateFile(IFormFile newFile, string oldFile);
    Response<string> DeleteFile(string fileName);
}