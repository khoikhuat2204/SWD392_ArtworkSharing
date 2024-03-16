using DataAccessLayer.Models;

namespace Services.Interface;

public interface IFavoriteArtworkService
{
    public List<Artwork> GetAllFavoriteArtworksOfAUser(int userId);

    public void Add(FavoriteArtwork favoriteArtwork);

    public void Remove(int favoriteArtworkID);
    
    public bool Exists(int favoriteArtworkId);
}