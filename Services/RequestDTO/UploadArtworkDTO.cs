using DataAccessLayer.Enum;
using DataAccessLayer.Models;

namespace Services.RequestDTO;

public class UploadArtworkDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public ArtworkStatus ArtworkStatus = ArtworkStatus.Available;
}