using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;
using Repository.Interface;

namespace Services.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IUserService _userService;
        private readonly IArtworkService _artworkService;

        public RatingService(IRatingRepository ratingRepository, 
            IUserService userService, IArtworkService artworkService)
        {
            _ratingRepository = ratingRepository;
            _userService = userService;
            _artworkService = artworkService;
        }
        public List<Rating> GetAll()
        {
            return _ratingRepository.GetAll().ToList();
        }

        public void Add(Rating rating)
        {
            _ratingRepository.Add(rating);
        }

        public void Update(Rating rating)
        {
            _ratingRepository.Update(rating);
        }

        public void Remove(Rating rating)
        {
            _ratingRepository.Delete(rating);
        }

        public float GetRatingOfAnArtwork(int artworkId)
        {
            var averageRating = _ratingRepository.GetAll()
                .Where(r => r.ArtworkId == artworkId).Select(r => r.Score).Average();
            float roundedNumber = (float)Math.Round(averageRating, 1);
            return roundedNumber;
        }

        public bool ValidateRating(CreateRatingDTO rating)
        {
            if (rating.Score < 1 || rating.Score > 5)
                throw new ArgumentException("Score must be between 1 and 5");
            if (_userService.GetById(rating.UserId) == null)
                throw new ArgumentException("User not found");
            if (_artworkService.GetById(rating.ArtworkId) == null)
                throw new ArgumentException("Artwork not found");
        
            return true;
        }
    }
}
