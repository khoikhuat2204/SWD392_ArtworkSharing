using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using DataAccessLayer.Models;
using System.Threading.Tasks;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("get-all-ratings")]
        public IActionResult GetAllRatings()
        {
            var ratings = _ratingService.GetAll();
            return Ok(ratings);
        }

        [HttpPost("add-new-rating")]
        public IActionResult AddRating([FromBody] Rating rating)
        {
            _ratingService.Add(rating);
            return Ok();
        }

        [HttpPut("update-rating/{id}")]
        public IActionResult UpdateRating([FromBody] Rating rating)
        {
            _ratingService.Update(rating);
            return Ok();
        }

        [HttpDelete("delete-rating/{id}")]
        public IActionResult RemoveRating([FromBody] Rating rating)
        {
            _ratingService.Remove(rating);
            return Ok();
        }
    }
}
