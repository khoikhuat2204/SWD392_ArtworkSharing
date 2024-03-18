using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Services.Services;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtworkTypeController : Controller
    {
        private readonly IArtworkTypeService _artworkTypeService;
        private readonly IMapper _mapper;

        public ArtworkTypeController(IArtworkTypeService artworkTypeService, IMapper mapper)
        {
            _artworkTypeService = artworkTypeService;
            _mapper = mapper;
        }

        [HttpGet("get-all-artwork-types")]
        public async Task<IActionResult> GetAllArtworkTypes()
        {
            var artworkTypes = _artworkTypeService.GetArtworkTypesNotDeleted();
            if (!artworkTypes.Any())
                return NotFound();
            var mappedArtworkTypes = artworkTypes.Select(p => _mapper.Map<ArtworkTypeDTO>(p)).ToList();
            return Ok(mappedArtworkTypes);
        }
    }
}
