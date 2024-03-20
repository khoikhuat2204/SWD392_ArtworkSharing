using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;

namespace Services.Interface;

public interface IArtworkTagService
{
    public bool AddArtworkTag(int artworkId, string tagName);
    public bool AddTagsToArtwork(CreateArtworkTagDTO createArtworkTagDto);
    public bool EditArtworkTag(EditArtworkTagDTO editArtworkTagDto);
    public List<ArtworkTag> GetTagsByArtworkId(int artworkId);

}