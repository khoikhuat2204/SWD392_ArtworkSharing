using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;

namespace Services.Interface;

public interface IArtworkTagService
{
    public bool AddArtworkTag(int artworkId, int tagId);
    public bool AddTagsToArtwork(CreateArtworkTagDTO createArtworkTagDto);
    
}