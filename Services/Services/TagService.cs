using DataAccessLayer.Models;
using Repository.Interface;
using Services.Interface;

namespace Services.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
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
}