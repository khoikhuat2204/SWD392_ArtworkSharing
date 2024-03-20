using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.DTOs.ResponseDTO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Repository.Interface;
using Services.Extensions;
using Services.Interface;
using DataAccessLayer.DTOs.ResponseDTO;

namespace SWD392.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtworkController : Controller
{
    private readonly IArtworkService _artworkService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IAzureService _azureService;
    private readonly IRatingService _ratingService;

    public ArtworkController(IArtworkService artworkService, IUserService userService, IMapper mapper, IAzureService azureService, IRatingService ratingService)
    {
        _artworkService = artworkService;
        _mapper = mapper;
        _azureService = azureService;
        _ratingService = ratingService;
        _userService = userService;
    }

    [HttpGet("get-all-artworks")]
    public async Task<IActionResult> GetAllArtworks()
    {
        var artworks = _artworkService.GetAll();
        if (!artworks.Any())
            return Ok("No artworks found");
        var mappedArtworks = artworks.Select(p => _mapper.Map<ArtworkDTO>(p)).ToList();
        return Ok(mappedArtworks);
    }
    [HttpGet("get-all-artworks/type/{id}")]
    public async Task<IActionResult> GetArtworksByType(int id)
    {
        var artworks = _artworkService.GetAllByArtworkType(id);
        if (!artworks.Any())
            return Ok("No artworks found");
        var mappedArtworks = artworks.Select(p => _mapper.Map<ArtworkDTO>(p)).ToList();
        return Ok(mappedArtworks);
    }

    [HttpGet("get-artworks/user/{id}")]
    public async Task<IActionResult> GetArtworksByUserId(int id)
    {
        var artworks = _artworkService.GetAllByUserId(id);
        if (!artworks.Any())
            return Ok("No artworks found");
        var mappedArtworks = artworks.Select(p => _mapper.Map<ArtworkDTO>(p)).ToList();
        return Ok(mappedArtworks);
    }
    
    [HttpGet("get-artwork/{id}")]
    public async Task<IActionResult> GetArtworksById(int id)
    {
        var artwork = _artworkService.GetAll().Find(x => x.Id.Equals(id));
        if (artwork == null)
             return Ok("No artworks found");
        var creator = _userService.GetAllCreator().Find(x => x.Id.Equals(artwork.UserId));
        var mappedCreator = _mapper.Map<UserDTO>(creator);

           
        var mappedArtworks = _mapper.Map<ArtworkDTO>(artwork);
        mappedArtworks.Creator = mappedCreator;
        return Ok(mappedArtworks);
    }

    [HttpPost("add-artwork")]
    [Authorize(Roles = "Creator")]
    public async Task<IActionResult> AddArtwork([FromForm] UploadArtworkDTO uploadArtworkDto)
    {
        var userId = User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "userId")?.Value ?? string.Empty;

        if (_artworkService.CheckSubscriptionForUpload(Int32.Parse(userId)))
        {
            var imageUrls = new List<string?>();
            var imageExtension = ImageExtension.ImageExtensionChecker(uploadArtworkDto.ImageUploadRequest.FileName);
            var uri = (await _azureService.UploadImage(uploadArtworkDto.ImageUploadRequest, null, "post", imageExtension, false))?.Blob.Uri;
            imageUrls.Add(uri);

            var createdArtwork = new Artwork()
            {
                UserId = Int32.Parse(userId),
                CreatedDate = DateTime.Now,
                Name = uploadArtworkDto.Name,
                Description = uploadArtworkDto.Description,
                Price = uploadArtworkDto.Price,
                TypeId = uploadArtworkDto.TypeId,
                ArtworkStatus = uploadArtworkDto.ArtworkStatus,
                IsDeleted = false,
                ImagePath = imageUrls[0],
            };
            _artworkService.Add(createdArtwork);
            return Ok(createdArtwork);
        }
        else
        {
            return BadRequest("You don't have a subscription or you have reached your upload limit for today or you have exceeded your total upload limit");
        }
    }

    [Authorize(Roles = "Creator")]
    [HttpPut("update-artwork/{id}")]
    public async Task<IActionResult> UpdateArtwork(int id, [FromBody] UpdateArtworkDTO updateArtworkDto)
    {
        var existingArtwork = _artworkService.GetAll().FirstOrDefault(a => a.Id == id);
        if (id == 0)
        {
            return BadRequest("Id must not be 0");
        }
        if (existingArtwork == null)
        {
            return Ok("No artworks found");
        }

        existingArtwork.Name = updateArtworkDto.Name;
        existingArtwork.Description = updateArtworkDto.Description;
        existingArtwork.Price = updateArtworkDto.Price;
        existingArtwork.TypeId = updateArtworkDto.TypeId;
        existingArtwork.IsDeleted = updateArtworkDto.IsDeleted;

        _artworkService.Update(existingArtwork);
        return NoContent();
    }

    /*    [HttpDelete("delete-artwork/{id}")]
        public async Task<IActionResult> DeleteArtwork(int id)
        {
            var artwork = _artworkService.GetAll().FirstOrDefault(a => a.Id == id);
            if (artwork == null)
            {
                return NotFound();
            }
            _artworkService.Remove(artwork);
            return NoContent();
        }*/

    [HttpDelete("delete-artwork/{id}")]
    public async Task<IActionResult> DeleteArtwork(int id)
    {
        var result = _artworkService.DeleteByArtwork(id);
        if (result)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }



    [HttpPost("search-by-tags")]
    public async Task<IActionResult> SearchByTags([FromBody]SearchByTagsDTO tags)
    {
        var artworks = _artworkService.SearchByTags(tags);
        if (!artworks.Any())
            return Ok("no artworks found with these tags");
        var mappedArtworks = artworks.Select(p => _mapper.Map<ArtworkDTO>(p)).ToList();
        return Ok(mappedArtworks);
    }
    
    [HttpGet("search-by-name/{name}")]
    public async Task<IActionResult> SearchByName(string name)
    {
        var artworks = _artworkService.SearchByName(name);
        if (!artworks.Any())
            return Ok("no artworks found with this name");
        var mappedArtworks = artworks.Select(p => _mapper.Map<ArtworkDTO>(p)).ToList();
        return Ok(mappedArtworks);
    }
    [HttpGet("get-all-artwork-with-rating")]
    public IActionResult GetAllArtworkWithRating()
    {
        var artworks = _artworkService.GetAll();
        if(artworks.Count == 0)
            return Ok("No artworks found");
            
        var mappedArtworks = _mapper.Map<List<ArtworkDetailDTO>>(artworks);
        foreach (var artwork in mappedArtworks)
        {
            artwork.Rating = _ratingService.GetRatingOfAnArtwork(artwork.Id);
        }
        return Ok(mappedArtworks);
    }
        
    [HttpGet("get-artwork-with-rating/{artworkId}")]
    public IActionResult GetRatingOfAnArtwork(int artworkId)
    {
        if (_artworkService.GetById(artworkId) == null)
            return BadRequest("Artwork not found");
            
        var rating = _ratingService.GetRatingOfAnArtwork(artworkId);
        var artwork = _artworkService.GetById(artworkId);
        var mappedArtworks = _mapper.Map<ArtworkDetailDTO>(artwork);
        mappedArtworks.Rating = rating;
        return Ok(mappedArtworks);
    }
}