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

        [HttpGet]
        public IActionResult GetAllReservations()
        {
            var reservations = _reservationService.GetAll();
            return Ok(reservations);
        }

        [HttpPost]
        public IActionResult AddReservation([FromBody] Reservation reservation)
        {
            _reservationService.Add(reservation);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateReservation([FromBody] Reservation reservation)
        {
            _reservationService.Update(reservation);
            return Ok();
        }

        [HttpDelete]
        public IActionResult RemoveReservation([FromBody] Reservation reservation)
        {
            _reservationService.Remove(reservation);
            return Ok();
        }
    }
}
