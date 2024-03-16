using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace SWD392.Controllers;

[ApiController]
[Route("/")]
public class FavoriteArtworkController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IFavoriteArtworkService _favoriteArtworkService;
    private readonly IArtworkService _artworkService;

    public FavoriteArtworkController(IMapper mapper, IFavoriteArtworkService favoriteArtworkService,
        IArtworkService artworkService)
    {
        _mapper = mapper;
        _favoriteArtworkService = favoriteArtworkService;
        _artworkService = artworkService;
    }
    
    [HttpGet("get-all-favorite-artworks-of-a-user")]
    public IActionResult GetAllFavoriteArtworksOfAUser(int userId)
    {
        var favoriteArtworks = _favoriteArtworkService.GetAllFavoriteArtworksOfAUser(userId);
        if (!favoriteArtworks.Any())
            return Ok("No favorite artworks found");
        var mappedFavoriteArtworks = _mapper.Map<List<ArtworkDTO>>(favoriteArtworks);
        return Ok(mappedFavoriteArtworks);
    }
    
    [HttpPost("add-favorite-artwork")]
    public IActionResult AddFavoriteArtwork(CreateFavoriteArtworkDTO createFavoriteArtworkDto)
    {
        if(_artworkService.GetAll().Any(a => a.Id != createFavoriteArtworkDto.ArtworkId))
            return BadRequest("Artwork does not exist");
        _favoriteArtworkService.Add(_mapper.Map<FavoriteArtwork>(createFavoriteArtworkDto));
        return Ok("Favorite artwork added");
    }
    
    [HttpDelete("remove-favorite-artwork")]
    public IActionResult RemoveFavoriteArtwork(int favoriteArtworkId)
    {
        if(!_favoriteArtworkService.Exists(favoriteArtworkId))
            return BadRequest("Favorite artwork does not exist");
        _favoriteArtworkService.Remove(favoriteArtworkId);
        return Ok("Favorite artwork removed");
    }
}