using DataAccessLayer.Custom;
using Microsoft.AspNetCore.Http;
using Repository.Interface;
using Repository.Repos;
using Services.Interface;

namespace Services.Services;

public class AzureService : IAzureService
{
    private readonly IAzureStorageRepository _azureStorageRepository;

    public AzureService(IAzureStorageRepository azureStorageRepository)
    {
        _azureStorageRepository = azureStorageRepository;
    }
    public async Task<BlobResponse?> UploadImage(IFormFile? file, string? fileName, string containerName, string? fileExtension, bool isPrivate)
    {
        return await _azureStorageRepository.UpdateFileAsync(file, fileName, containerName, fileExtension, isPrivate);
    }
}