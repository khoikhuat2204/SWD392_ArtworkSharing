namespace DataAccessLayer.DTOs.ResponseDTO;

public class PackageDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int UploadsPerDay { get; set; }
    public int TotalUploads { get; set; }
    public bool IsDeleted { get; set; }
}