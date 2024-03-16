using DataAccessLayer.Custom;
using Microsoft.AspNetCore.Http;


namespace Repository.Interface;

public interface IAzureStorageRepository
{
    public Task<BlobResponse?> UpdateFileAsync(IFormFile? file, string? fileName, string containerName,
        string? fileExtension, bool isPrivate);

}