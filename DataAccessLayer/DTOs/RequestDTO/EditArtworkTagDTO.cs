namespace DataAccessLayer.DTOs.RequestDTO
{
    public class EditArtworkTagDTO
    {
        public int ArtworkId { get; set; }
        public List<string> Tags { get; set; }
    }
}
