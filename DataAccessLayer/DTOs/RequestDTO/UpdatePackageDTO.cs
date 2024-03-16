namespace DataAccessLayer.DTOs.RequestDTO;

public class UpdatePackageDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int UploadsPerDay { get; set; }
    public int TotalUploads { get; set; }
    public bool IsDeleted { get; set; }
}