namespace DataAccessLayer.DTOs.RequestDTO;

public class ReservationRequestDTO
{
    // public int Id { get; set; }
    //public DateTime? Date { get; set; }
    public int UserId { get; set; }
    public int PackageId { get; set; }
    //public bool IsDeleted { get; set; }
    public int ArtworkId { get; set; }
}