using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Services.Interface;
using Stripe;

namespace Services.Services;

public class ArtworkTagService : IArtworkTagService
{
    private readonly IArtworkTagRepository _artworkTagRepository;
    private readonly ITagRepository _tagRepository;

    public ArtworkTagService(IArtworkTagRepository artworkTagRepository)
    {
        _artworkTagRepository = artworkTagRepository;
    }

    public bool AddArtworkTag(int artworkId, string tagName)
    {
        Tag? tag = _tagRepository.FindByName(tagName);
        if (tag == null)
        {
            _tagRepository.Add(new Tag() { Name = tagName, IsDeleted = false });
        }
        tag = _tagRepository.FindByName(tagName)!;
        var artworkTag = new ArtworkTag
        {
            ArtworkId = artworkId,
            TagId = tag.Id
        };
        return _artworkTagRepository.Add(artworkTag);
    }

    public bool AddTagsToArtwork(CreateArtworkTagDTO createArtworkTagDto)
    {
        foreach (var tagName in createArtworkTagDto.Tags)
        {
            if (!AddArtworkTag(createArtworkTagDto.ArtworkId, tagName))
            {
                return false;
            }
        }
        return true;
    }

    // Not optimized I know
    public bool EditArtworkTag(EditArtworkTagDTO editArtworkTagDto)
    {
        List<ArtworkTag> currTags = _artworkTagRepository.GetAll()
            .Where(at => at.ArtworkId == editArtworkTagDto.ArtworkId)
            .ToList();

        foreach (ArtworkTag tag in currTags)
        {
            _artworkTagRepository.Delete(tag);
        }

        foreach (var tagName in editArtworkTagDto.Tags)
        {
            if (!AddArtworkTag(editArtworkTagDto.ArtworkId, tagName))
            {
                return false;
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