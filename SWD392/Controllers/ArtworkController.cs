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
    
}