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

        public RatingController(IRatingService ratingService, IMapper mapper)
        {
            _ratingService = ratingService;
            _mapper = mapper;
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
    }
}
