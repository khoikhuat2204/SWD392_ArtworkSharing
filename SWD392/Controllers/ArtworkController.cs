using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Repository.Interface;
using Services.Extensions;
using Services.Interface;

namespace SWD392.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtworkController : Controller
{
    private readonly IArtworkService _artworkService;
    private readonly IMapper _mapper;
    private readonly IAzureService _azureService;

    public ArtworkController(IArtworkService artworkService, IMapper mapper, IAzureService azureService)
    {
        _artworkService = artworkService;
        _mapper = mapper;
        _azureService = azureService;
    }

    [HttpGet("get-all-artworks")]
    public async Task<IActionResult> GetAllArtworks()
    {
        var artworks = _artworkService.GetAll();
        if (!artworks.Any())
            return NotFound();
        var mappedArtworks = artworks.Select(p => _mapper.Map<ArtworkDTO>(p)).ToList();
        return Ok(mappedArtworks);
    }
    
    [HttpGet("get-artworks/user/{id}")]
    public async Task<IActionResult> GetArtworksByUserId(int id)
    {
        var artworks = _artworkService.GetAllByUserId(id);
        if (!artworks.Any())
            return NotFound();
        var mappedArtworks = artworks.Select(p => _mapper.Map<ArtworkDTO>(p)).ToList();
        return Ok(mappedArtworks);
    }
    
    [HttpGet("get-artwork/{id}")]
    public async Task<IActionResult> GetArtworksById(int id)
    {
        var artwork = _artworkService.GetAll().Find(x => x.Id.Equals(id));
        if (artwork == null)
            return NotFound();
        var mappedArtworks = _mapper.Map<ArtworkDTO>(artwork);
        return Ok(mappedArtworks);
    }

    [HttpPost("add-artwork")]
    [Authorize(Roles = "Creator")]
    public async Task<IActionResult> AddArtwork([FromForm] UploadArtworkDTO uploadArtworkDto)
    {
        var userId = User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "userId")?.Value ?? string.Empty;

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
        return Ok();
    }

    [Authorize(Roles = "Creator")]
    [HttpPut("update-artwork/{id}")]
    public async Task<IActionResult> UpdateArtwork(int id, [FromBody] UpdateArtworkDTO updateArtworkDto)
    {
        var existingArtwork = _artworkService.GetAll().FirstOrDefault(a => a.Id == id);
        if (id == 0)
        {
            return BadRequest();
        }
        if (existingArtwork == null)
        {
            return NotFound();
        }

        existingArtwork.Name = updateArtworkDto.Name;
        existingArtwork.Description = updateArtworkDto.Description;
        existingArtwork.Price = updateArtworkDto.Price;
        existingArtwork.TypeId = updateArtworkDto.TypeId;
        existingArtwork.IsDeleted = updateArtworkDto.IsDeleted;

        _artworkService.Update(existingArtwork);
        return NoContent();
    }

    [HttpDelete("delete-artwork/{id}")]
    public async Task<IActionResult> DeleteArtwork(int id)
    {
        var artwork = _artworkService.GetAll().FirstOrDefault(a => a.Id == id);
        if (artwork == null)
        {
            return NotFound();
        }
        _artworkService.Remove(artwork);
        return NoContent();
    }


}