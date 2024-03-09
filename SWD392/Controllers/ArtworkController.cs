using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Services.Interface;

namespace SWD392.Controllers;

[ApiController]
[Route("/")]
public class ArtworkController : Controller
{
    private readonly IArtworkService _artworkService;
    private readonly IMapper _mapper;

    public ArtworkController(IArtworkService artworkService, IMapper mapper)
    {
        _artworkService = artworkService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllArtworks()
    {
        return  Ok(_artworkService.GetAll());
    }

    [HttpPost("add-artwork")]
    [Authorize(Roles = "Creator")]
    public async Task<IActionResult> AddArtwork([FromForm] UploadArtworkDTO uploadArtworkDto)
    {
        var userId = User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "userId")?.Value ?? string.Empty;
        var createdArtwork = new Artwork()
        {
            UserId = Int32.Parse(userId),
            CreatedDate = DateTime.Now,
            Name = uploadArtworkDto.Name,
            Description = uploadArtworkDto.Description,
            TypeId = uploadArtworkDto.TypeId,
            ArtworkStatus = uploadArtworkDto.ArtworkStatus,
            IsDeleted = false,
        };
        
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArtwork(int id, [FromBody] Artwork artwork)
    {
        var existingArtwork = _artworkService.GetAll().FirstOrDefault(a => a.Id == id);
        if (existingArtwork == null)
        {
            return NotFound();
        }
        _artworkService.Update(artwork);
        return NoContent();
    }

    [HttpDelete("{id}")]
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