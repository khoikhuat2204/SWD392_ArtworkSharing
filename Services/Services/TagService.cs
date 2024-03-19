using DataAccessLayer.Models;
using Repository.Interface;
using Services.Interface;

namespace Services.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IArtworkTagService _artworkTagService;

    public TagService(ITagRepository tagRepository, IArtworkTagService artworkTagService)
    {
        _tagRepository = tagRepository;
        _artworkTagService = artworkTagService;
    }

    public bool AddTag(Tag tag)
    {
        try
        {
            return _tagRepository.Add(tag);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public bool UpdateTag(Tag tag)
    {
        try
        {
            return _tagRepository.Update(tag);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public List<Tag> GetAllTags()
    {
        return _tagRepository.GetAll().ToList();
    }
    
    public List<Tag> GetTagsByArtworkId(int artworkId)
    {
        var artworkTags = _artworkTagService.GetTagsByArtworkId(artworkId).ToList();
        List<Tag> tags = new List<Tag>();
        foreach (var artworkTag in artworkTags)
        {
            var tag = GetAllTags().FirstOrDefault(t => t.Id == artworkTag.TagId);
            tags.Add(tag);
        }

        return tags;
    }

    public bool Exists(int tagId)
    {
        return _tagRepository.GetAll().Any(t => t.Id == tagId);
    }
}