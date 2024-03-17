using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace Services.Interface
{
    public interface IRatingService
    {
        public List<Rating> GetAll();

        public void Add(Rating rating);

        public void Update(Rating rating);

        public void Remove(Rating rating);
        public float GetRatingOfAnArtwork(int artworkId);
    }
}
