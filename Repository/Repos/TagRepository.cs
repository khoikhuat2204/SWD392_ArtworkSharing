using DataAccessLayer.Models;
using Repository.BaseRepository;
using Repository.Interface;

namespace Repository.Repos
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public Tag? FindByName(string name)
        {
            return GetAll().FirstOrDefault(x => x.Name == name);
        }
    }
}
