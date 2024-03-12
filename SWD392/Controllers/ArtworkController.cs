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
[Route("/")]
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

    [HttpPost("add-artwork")]
    [Authorize(Roles = "Creator")]
    public async Task<IActionResult> AddArtwork([FromForm] UploadArtworkDTO uploadArtworkDto)
    {
        var userId = User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "userId")?.Value ?? string.Empty;
    
        var imageUrls = new List<string?>();
        foreach (var image in uploadArtworkDto.ImageUploadRequest)
        {
            var imageExtension = ImageExtension.ImageExtensionChecker(image.FileName);
            var uri = (await _azureService.UploadImage(image, null, "post", imageExtension, false))?.Blob.Uri;
            imageUrls.Add(uri);
        }
        
        var createdArtwork = new Artwork()
        {
            UserId = Int32.Parse(userId),
            CreatedDate = DateTime.Now,
            Name = uploadArtworkDto.Name,
            Description = uploadArtworkDto.Description,
            TypeId = uploadArtworkDto.TypeId,
            ArtworkStatus = uploadArtworkDto.ArtworkStatus,
            IsDeleted = false,
            ImagePath = imageUrls[0],
        };
        
        _artworkService.Add(createdArtwork);
        return Ok();
    }

    [HttpPut("update-artwork/{id}")]
    public async Task<IActionResult> UpdateArtwork(int id, [FromBody] UpdateArtworkDTO updateArtworkDto)
    {
        var existingArtwork = _artworkService.GetAll().FirstOrDefault(a => a.Id == id);
        if (existingArtwork == null)
        {
            return NotFound();
        }
        
        existingArtwork.Name = updateArtworkDto.Name;
        existingArtwork.Description = updateArtworkDto.Description;
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