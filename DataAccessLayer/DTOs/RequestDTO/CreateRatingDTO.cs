namespace DataAccessLayer.DTOs.RequestDTO;

public class CreateRatingDTO
{
    public int Score { get; set; }
    public int UserId { get; set; }
    public int ArtworkId { get; set; }
}