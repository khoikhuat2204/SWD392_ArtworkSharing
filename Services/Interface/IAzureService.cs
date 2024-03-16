using DataAccessLayer.Custom;
using Microsoft.AspNetCore.Http;

namespace Services.Interface;

public interface IAzureService
{
    public Task<BlobResponse?> UploadImage(IFormFile? file, string? fileName, string containerName,
        string? fileExtension, bool isPrivate);
}