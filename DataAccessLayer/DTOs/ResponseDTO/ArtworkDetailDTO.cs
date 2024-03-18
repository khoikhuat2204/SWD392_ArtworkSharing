using DataAccessLayer.Enum;

namespace DataAccessLayer.DTOs.ResponseDTO;

public class ArtworkDetailDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImagePath { get; set; }
    public int TypeId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public ArtworkStatus ArtworkStatus { get; set; }
    public float Rating { get; set; }
}