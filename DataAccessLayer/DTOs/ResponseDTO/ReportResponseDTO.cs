namespace DataAccessLayer.DTOs.ResponseDTO;

public class ReportResponseDTO
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public int CauseId { get; set; }
    public int UserId { get; set; }
    public int ArtworkId { get; set; }
}