namespace DataAccessLayer.DTOs.RequestDTO;

public class UpdateArtworkDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int TypeId { get; set; }
    public bool IsDeleted { get; set; }
}