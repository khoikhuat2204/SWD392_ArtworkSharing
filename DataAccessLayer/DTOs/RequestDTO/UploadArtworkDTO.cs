using DataAccessLayer.Enum;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.DTOs.RequestDTO;

public class UploadArtworkDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public int TypeId { get; set; }
    public ArtworkStatus ArtworkStatus = ArtworkStatus.Available;
    public IFormFileCollection? ImageUploadRequest { get; set; }
}