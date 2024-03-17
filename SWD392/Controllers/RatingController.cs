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
                if(rating.Score < 1 || rating.Score > 5)
                    return BadRequest("Score must be between 1 and 5");
                if(_userService.GetById(rating.UserId) == null)
                    return BadRequest("User not found");
                if (_artworkService.GetById(rating.ArtworkId) == null)
                    return BadRequest("Artwork not found");
                
                var mappedRating = _mapper.Map<Rating>(rating);
                _ratingService.Add(mappedRating);
                return Ok("Rating added successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        
        [HttpGet("get-rating-of-an-artwork/{artworkId}")]
        public IActionResult GetRatingOfAnArtwork(int artworkId)
        {
            if (_artworkService.GetById(artworkId) == null)
                return BadRequest("Artwork not found");
            var rating = _ratingService.GetRatingOfAnArtwork(artworkId);
            return Ok(rating);
        }
    }
}
