﻿using DataAccessLayer.Models;
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

        public IQueryable<Artwork> SearchByTags(List<int> tagIds)
        {
            var artworks = GetAll().Include(x => x.Tags).ToList();
            foreach (var tagId in tagIds)
            {
                artworks = artworks.Where(a => a.Tags != null && a.Tags.Any(t => t.Id == tagId)).ToList();
            }
            return artworks.AsQueryable();
        }
    }
}
