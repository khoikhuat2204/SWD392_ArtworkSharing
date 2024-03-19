using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
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

    public bool EditArtworkTag(CreateArtworkTagDTO editArtworkTagDto)
    {
        var artworkTags = _artworkTagRepository.GetAll()
            .Where(at => at.ArtworkId == editArtworkTagDto.ArtworkId)
            .AsNoTracking().ToList();
        
        var tagsToRemove = artworkTags.Where(at => !editArtworkTagDto.TagId.Contains(at.TagId)).ToList();
        foreach (var tag in tagsToRemove)
        {
            _artworkTagRepository.Delete(tag);
        }
        
        // Add new tags
        foreach (var tagId in editArtworkTagDto.TagId)
        {
            // Check if the tag is already associated with the artwork
            if (!artworkTags.Any(at => at.TagId == tagId))
            { 
                var newArtworkTag = new ArtworkTag { ArtworkId = editArtworkTagDto.ArtworkId, TagId = tagId };
                _artworkTagRepository.Add(newArtworkTag);
            }
        }
        return true;
    }

    // get tag by artworkId
    public List<ArtworkTag> GetTagsByArtworkId(int artworkId)
    {
        return _artworkTagRepository.GetAll().Where(a => a.ArtworkId == artworkId).ToList();
    }
}