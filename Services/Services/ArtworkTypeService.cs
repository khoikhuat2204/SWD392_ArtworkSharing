using DataAccessLayer.Models;
using Repository.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ArtworkTypeService : IArtworkTypeService
    {
        private readonly IArtworkTypeRepository _artworkTypeRepository;

        public ArtworkTypeService(IArtworkTypeRepository artworkTypeRepository)
        {
            _artworkTypeRepository = artworkTypeRepository;
        }

        public List<ArtworkType> GetArtworkTypesNotDeleted()
        {
            return _artworkTypeRepository.GetArtworkTypesNotDeleted().ToList();
        }
    }
}
