using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;
using Repository.Interface;
using Services.Interface;

namespace Services.Services;

public class ArtworkTagService : IArtworkTagService
{
    private readonly IArtworkTagRepository _artworkTagRepository;

    public ArtworkTagService(IArtworkTagRepository artworkTagRepository)
    {
        _artworkTagRepository = artworkTagRepository;
    }
    public bool AddArtworkTag(int artworkId, int tagId)
    {
        var artworkTag = new ArtworkTag
        {
            ArtworkId = artworkId,
            TagId = tagId
        };
        return _artworkTagRepository.Add(artworkTag);
    }

    public bool AddTagsToArtwork(CreateArtworkTagDTO createArtworkTagDto)
    {
        var result = true;
        foreach (var tagId in createArtworkTagDto.TagId)
        {
            result = AddArtworkTag(createArtworkTagDto.ArtworkId, tagId);
            if (!result)
            {
                return false;
            }
        }
        return result;
    }
}