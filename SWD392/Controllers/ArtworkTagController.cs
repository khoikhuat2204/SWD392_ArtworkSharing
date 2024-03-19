using DataAccessLayer.DTOs.RequestDTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace SWD392.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ArtworkTagController : ControllerBase
{
    private readonly IArtworkTagService _artworkTagService;
    private readonly ITagService _tagService;
    private readonly IArtworkService _artworkService;

    public ArtworkTagController(IArtworkTagService artworkTagService, ITagService tagService, IArtworkService artworkService)
    {
        _artworkTagService = artworkTagService;
        _tagService = tagService;
        _artworkService = artworkService;
    }
    
    [HttpPost("add-tag-to-artwork")]
    public async Task<IActionResult> AddTagToArtwork(CreateArtworkTagDTO createArtworkTagDto)
    {
        if(_artworkService.GetById(createArtworkTagDto.ArtworkId) == null)
            return BadRequest("Artwork not found");
        
        var result = _artworkTagService.AddTagsToArtwork(createArtworkTagDto);
        if (result)
        {
            return Ok("Tags added to artwork");
        }
        return BadRequest("Failed to add tags to artwork");
    }
}