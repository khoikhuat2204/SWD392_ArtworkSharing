using DataAccessLayer.Enum;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.DTOs.RequestDTO;

public class UploadArtworkDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public int TypeId { get; set; }
    public ArtworkStatus ArtworkStatus = ArtworkStatus.Available;
    public IFormFile? ImageUploadRequest { get; set; }
}