using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;

namespace Services.Interface
{
    public interface IArtworkService
    {
        public List<Artwork> GetAll();
        public List<Artwork> GetAllByUserId(int id);
        public void Add(Artwork artwork);

        public void Update(Artwork artwork);

        public void Remove(Artwork artwork);
        public List<Artwork> SearchByTags(SearchByTagsDTO tags);
        public List<Artwork> SearchByName(string name);
        public Artwork GetById(int id);

    }
}
