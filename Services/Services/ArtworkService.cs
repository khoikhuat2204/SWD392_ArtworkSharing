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
        
    }
}
