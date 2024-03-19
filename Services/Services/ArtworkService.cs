using Services.Interface;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Repos;

namespace Services.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly IArtworkRepository _artworkRepository;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IReservationRepository _reservationRepository;
        private readonly IRatingRepository _ratingRepository;

        public ArtworkService(IArtworkRepository artworkRepository, ISubscriptionService subscriptionService, 
            IReservationRepository reservationRepository, IRatingRepository ratingRepository)
        {
            _artworkRepository = artworkRepository;
            _subscriptionService = subscriptionService;
            _reservationRepository = reservationRepository;
            _ratingRepository = ratingRepository;
        }

        public List<Artwork> GetAll()
        {
            return _artworkRepository.GetAll().ToList();
        }

        public void Add(Artwork artwork)
        {
            _artworkRepository.Add(artwork);
        }

        public bool Update(Artwork artwork)
        {
            try
            {
                _artworkRepository.Update(artwork);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public void Remove(Artwork artwork)
        {
             _artworkRepository.Delete(artwork);
        }

        public bool DeleteByArtwork(int artworkId)
        {
            // Get the artwork by id
            var artwork = _artworkRepository.GetById(artworkId);
            if (artwork == null)
            {
                return false;
            }

            // Get all reservations for this artwork and set their IsDeleted flag to true
            var reservations = _reservationRepository.GetAll().Where(r => r.ArtworkId == artworkId).ToList();
            foreach (var reservation in reservations)
            {
                reservation.IsDeleted = true;
                _reservationRepository.Update(reservation);
            }

            // Get all ratings for this artwork and set their IsDeleted flag to true
            var ratings = _ratingRepository.GetAll().Where(r => r.ArtworkId == artworkId).ToList();
            foreach (var rating in ratings)
            {
                rating.IsDeleted = true;
                _ratingRepository.Update(rating);
            }

            // Set the IsDeleted flag to true for the artwork
            artwork.IsDeleted = true;

            // Update the artwork in the database
            _artworkRepository.Update(artwork);

            return true;
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

        public bool SellArtwork(Artwork artwork)
        {
            return _artworkRepository.MarkAsSold(artwork);
        }
    }
}
