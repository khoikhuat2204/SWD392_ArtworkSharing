using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;

namespace Services.Interface
{
    public interface IArtworkService
    {
        public List<Artwork> GetAll();
        public List<Artwork> GetAllByUserId(int id);
        public void Add(Artwork artwork);
        public void Update(Artwork artwork);
        public void Remove(Artwork artwork);
        
        public bool CheckSubscriptionForUpload(int userId);
        public List<Artwork> SearchByTags(SearchByTagsDTO tags);
        public List<Artwork> SearchByName(string name);
        public Artwork GetById(int id);

    }
}
