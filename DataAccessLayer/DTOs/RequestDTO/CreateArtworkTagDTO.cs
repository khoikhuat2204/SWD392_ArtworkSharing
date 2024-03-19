namespace DataAccessLayer.DTOs.RequestDTO;

public class CreateArtworkTagDTO
{
    public int ArtworkId { get; set; }
    public List<int> TagId { get; set; }
}