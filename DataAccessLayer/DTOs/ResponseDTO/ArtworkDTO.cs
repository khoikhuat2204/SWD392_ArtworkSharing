using DataAccessLayer.DTOs.ResponseDTO;
using DataAccessLayer.Enum;
using DataAccessLayer.Models;

namespace DataAccessLayer.DTOs.RequestDTO;

public class ArtworkDTO
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

    public UserDTO? Creator { get; set; } 
}