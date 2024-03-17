using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Repository.BaseRepository;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos
{
    public class ArtworkRepository : BaseRepository<Artwork>, IArtworkRepository
    {
        public IQueryable<Artwork> GetAllByUserId(int id)
        {
            return GetAll().Where(x => x.UserId == id);
        }

        public Artwork? GetById(int id)
        {
            return GetAll().FirstOrDefault(artwork => artwork.Id == id);
        }
    }
}
