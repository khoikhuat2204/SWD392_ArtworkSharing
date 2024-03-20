using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Repository.BaseRepository;
using Repository.Interface;

namespace Repository.Repos
{
    public class ArtworkRepository : BaseRepository<Artwork>, IArtworkRepository
    {
        public IQueryable<Artwork> GetAllByUserId(int id)
        {
            return GetAll().Where(x => x.UserId == id);
        }
        public IQueryable<Artwork> GetAllByArtworkType(int typeId)
        {
            return GetAll().Where(x => x.TypeId.Equals(typeId));
        }

        public Artwork? GetById(int id)
        {
            return GetAll().FirstOrDefault(artwork => artwork.Id == id);
        }

        public IQueryable<Artwork> SearchByName(string name)
        {
            return GetAll().Where(x => (x.Name ?? string.Empty).Contains(name));
        }

        public IQueryable<Artwork> SearchByTags(List<int> tagIds)
        {
            var artworks = GetAll().Include(x => x.Tags).ToList();
            foreach (var tagId in tagIds)
            {
                artworks = artworks.Where(a => a.Tags != null && a.Tags.Any(t => t.Id == tagId)).ToList();
            }
            return artworks.AsQueryable();
        }
    }
}
