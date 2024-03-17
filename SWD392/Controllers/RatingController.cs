using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using DataAccessLayer.Models;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.DTOs.ResponseDTO;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IArtworkService _artworkService;

        public RatingController(IRatingService ratingService, IMapper mapper,
            IUserService userService, IArtworkService artworkService)
        {
            _ratingService = ratingService;
            _mapper = mapper;
            _userService = userService;
            _artworkService = artworkService;
        }

        [HttpGet("get-all-ratings")]
        public IActionResult GetAllRatings()
        {
            var ratings = _ratingService.GetAll();
            if(ratings.Count == 0)
                return Ok("No ratings found");
            var mappedRatings = _mapper.Map<List<RatingResponseDTO>>(ratings);
            return Ok(mappedRatings);
        }

        [HttpPost("add-new-rating")]
        public IActionResult AddRating([FromBody] CreateRatingDTO rating)
        {
            try
            {
                _ratingService.ValidateRating(rating);
                
                var mappedRating = _mapper.Map<Rating>(rating);
                _ratingService.Add(mappedRating);
                return Ok("Rating added successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        [HttpGet("artwork/get-all-artwork-with-rating")]
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
        
        [HttpGet("artwork/get-artwork-with-rating/{artworkId}")]
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
}
