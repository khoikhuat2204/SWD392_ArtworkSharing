using DataAccessLayer.Enum;

namespace DataAccessLayer.DTOs.RequestDTO;

public class ArtworkDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public int TypeId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public ArtworkStatus ArtworkStatus { get; set; }
}