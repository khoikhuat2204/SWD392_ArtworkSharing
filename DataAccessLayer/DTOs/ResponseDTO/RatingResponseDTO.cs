namespace DataAccessLayer.DTOs.ResponseDTO;

public class RatingResponseDTO
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int UserId { get; set; }
    public int ArtworkId { get; set; }
    public bool IsDeleted { get; set; }
}