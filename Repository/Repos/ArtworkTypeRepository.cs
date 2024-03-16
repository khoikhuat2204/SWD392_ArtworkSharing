using DataAccessLayer.Models;
using Repository.BaseRepository;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos
{
    public class ArtworkTypeRepository : BaseRepository<ArtworkType>, IArtworkTypeRepository
    {
        public IQueryable<ArtworkType> GetArtworkTypesNotDeleted()
        {
            return GetAll().Where(x => !x.IsDeleted);
        }
    }
}
