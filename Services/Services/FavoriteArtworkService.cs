
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Services.Interface;

namespace Services.Services;

public class FavoriteArtworkService : IFavoriteArtworkService
{
    private readonly IFavoriteArtworkRepository _favoriteArtworkRepository;

    public FavoriteArtworkService(IFavoriteArtworkRepository favoriteArtworkRepository)
    {
        _favoriteArtworkRepository = favoriteArtworkRepository;
    }

    public List<Artwork> GetAllFavoriteArtworksOfAUser(int userId)
    {
        var favoriteArtworks = _favoriteArtworkRepository.GetAll().Where(f => f.UserId == userId)
            .Include(fa => fa.Artwork);
        
        return favoriteArtworks.Select(fa => fa.Artwork).ToList();
    }

    public void Add(FavoriteArtwork favoriteArtwork)
    {
        _favoriteArtworkRepository.Add(favoriteArtwork);
    }

    public void Remove(int favoriteArtworkId)
    {
        var favoriteArtwork = _favoriteArtworkRepository.GetAll().FirstOrDefault(f => f.Id == favoriteArtworkId);
        if (favoriteArtwork != null)
            _favoriteArtworkRepository.Delete(favoriteArtwork);
    }

    public bool Exists(int favoriteArtworkId)
    {
        return _favoriteArtworkRepository.GetAll().Any(f => f.Id == favoriteArtworkId);
    }
}