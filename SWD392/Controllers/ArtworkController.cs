using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Services.Interface;

namespace SWD392.Controllers;

[ApiController]
[Route("/")]
public class ArtworkController : Controller
{
    private readonly IArtworkService _artworkService;

    public ArtworkController(IArtworkService artworkService)
    {
        _artworkService = artworkService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllArtworks()
    {
        return  Ok(_artworkService.GetAll());
    }

    [HttpPost]
    public async Task<IActionResult> AddArtwork([FromBody] Artwork artwork)
    {
        _artworkService.Add(artwork);
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