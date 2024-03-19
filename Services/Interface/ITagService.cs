using DataAccessLayer.Models;

namespace Services.Interface;

public interface ITagService
{
    public bool AddTag(Tag tag);
    public bool UpdateTag(Tag tag);
    public List<Tag> GetAllTags();
}