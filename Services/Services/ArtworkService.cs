using Services.Interface;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Services.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly IArtworkRepository _artworkRepository;
        private readonly ISubscriptionService _subscriptionService;

        public ArtworkService(IArtworkRepository artworkRepository, ISubscriptionService subscriptionService)
        {
            _artworkRepository = artworkRepository;
            _subscriptionService = subscriptionService;
        }

        public List<Artwork> GetAll()
        {
            return _artworkRepository.GetAll().ToList();
        }

        public void Add(Artwork artwork)
        {
            _artworkRepository.Add(artwork);
        }

        public void Update(Artwork artwork)
        {
            _artworkRepository.Update(artwork);
        }

        public void Remove(Artwork artwork)
        {
             _artworkRepository.Delete(artwork);
        }


        public bool CheckSubscriptionForUpload(int userId)
        {
            var subscription = _subscriptionService.GetAllActiveSubscriptions()
                .Include(c => c.Package)
                .FirstOrDefault(c => c.UserId == userId);
            var artworksCreatedToday =
                GetAll().Where(c => c.UserId == userId && c.CreatedDate.Date == DateTime.Today).ToList();
            var allArtworks = GetAll().Where(c => c.UserId.Equals(userId) && c.CreatedDate > subscription.StartDate)
                .ToList();

            if (subscription == null)
                return false;
            if (artworksCreatedToday.Count() >= subscription.Package.UploadsPerDay)
            {
                return false;
            }

            if (allArtworks.Count() >= subscription.Package.TotalUploads)
            {
                return false;
            }

            return true;

        }

        public List<Artwork> SearchByTags(SearchByTagsDTO tags)
        {
            return _artworkRepository.SearchByTags(tags.TagId).ToList();
        }

        public List<Artwork> SearchByName(string name)
        {
            return _artworkRepository.SearchByName(name).ToList();
        }

        public Artwork GetById(int id)
        {
            return _artworkRepository.GetById(id);

        }

        public List<Artwork> GetAllByUserId(int id)
        {
            return _artworkRepository.GetAllByUserId(id).ToList();
        }

    }
}
