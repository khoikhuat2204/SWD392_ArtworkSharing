using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.DTOs.RequestDTO;

public class UpdateProfileDTO
{
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public IFormFile? ImageUploadRequest { get; set; }
}