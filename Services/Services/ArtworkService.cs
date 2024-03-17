using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Enum;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Services.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly IArtworkRepository _artworkRepository;

        public ArtworkService(IArtworkRepository artworkRepository)
        {
            _artworkRepository = artworkRepository;
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
        
        public List<Artwork> GetAllByUserId(int id)
        {
            return _artworkRepository.GetAllByUserId(id).ToList();
        }

    }
}
