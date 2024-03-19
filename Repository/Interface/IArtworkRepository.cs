using DataAccessLayer.Models;
using Repository.BaseRepository;

namespace Repository.Interface
{
    public interface IArtworkRepository : IBaseRepository<Artwork>
    {
        IQueryable<Artwork> GetAllByUserId(int id);
        Artwork? GetById(int id);
        IQueryable<Artwork> SearchByTags(List<int> tagIds);
        IQueryable<Artwork> SearchByName(string name);
        IQueryable<Artwork> GetAllByArtworkType(int typeId);
        IQueryable<Artwork> GetAllByArtworkName(string artworkName);
        public bool MarkAsSold(Artwork artwork);
    }
}
