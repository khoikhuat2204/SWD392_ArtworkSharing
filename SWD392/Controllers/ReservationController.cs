using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using DataAccessLayer.Models;
using System.Threading.Tasks;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("/")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("get-all-reservations")]
        public IActionResult GetAllReservations()
        {
            var reservations = _reservationService.GetAll();
            return Ok(reservations);
        }

        [HttpPost("add-reservation")]
        public IActionResult AddReservation([FromBody] Reservation reservation)
        {
            _reservationService.Add(reservation);
            return Ok();
        }

        [HttpPut("update-reservation/{id}")]
        public IActionResult UpdateReservation([FromBody] Reservation reservation)
        {
            _reservationService.Update(reservation);
            return Ok();
        }

        [HttpDelete("delete-reservation/{id}")]
        public IActionResult RemoveReservation([FromBody] Reservation reservation)
        {
            _reservationService.Remove(reservation);
            return Ok();
        }
    }
}
