using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;

namespace Services.Interface
{
    public interface IArtworkService
    {
        public List<Artwork> GetAll();
        public List<Artwork> GetAllByArtworkType(int typeId);

        public List<Artwork> GetAllByUserId(int id);
        public List<Artwork> GetAllByArtworkName(string artworkName);
        public void Add(Artwork artwork);
        public bool Update(Artwork artwork);
        public void Remove(Artwork artwork);
        public bool DeleteByArtwork(int artworkId);
        public bool CheckSubscriptionForUpload(int userId);
        public List<Artwork> SearchByTags(SearchByTagsDTO tags);
        public List<Artwork> SearchByName(string name);
        public Artwork GetById(int id);
        public bool SellArtwork(Artwork artwork);
    }
}
