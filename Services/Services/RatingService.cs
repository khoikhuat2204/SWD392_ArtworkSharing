using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Repository.Interface;

namespace Services.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
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
    }
}
