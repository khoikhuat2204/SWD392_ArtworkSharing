using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using DataAccessLayer.Models;
using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.DTOs.ResponseDTO;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IArtworkService _artworkService;
        private readonly IPackageService _packageService;

        public ReservationController(IReservationService reservationService, IMapper mapper
        , IUserService userService, IArtworkService artworkService, IPackageService packageService)
        {
            _reservationService = reservationService;
            _mapper = mapper;
            _userService = userService;
            _artworkService = artworkService;
            _packageService = packageService;
        }

        [HttpGet("get-all-reservations")]
        public IActionResult GetAllReservations()
        {
            var reservations = _reservationService.GetAll();
            if (reservations == null)
            {
                return BadRequest("No reservation found");
            }

            var result = _mapper.Map<List<ReservationResponseDTO>>(reservations);
            return Ok(result);
        }

        [HttpPost("add-reservation")]
        public IActionResult AddReservation([FromBody] ReservationRequestDTO reservation)
        {
            if (reservation == null)
            {
                return BadRequest();
            }
            
            if(_userService.GetById(reservation.UserId) == null)
            {
                return BadRequest("User not found");
            }
            
            if(_artworkService.GetAll().FirstOrDefault(a => a.Id == reservation.ArtworkId) == null)
            {
                return BadRequest("Artwork not found");
            }
            
            var mappedReservation = _mapper.Map<Reservation>(reservation);
            
            _reservationService.Add(mappedReservation);
            return Ok();
        }

        [HttpPut("update-reservation/{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] ReservationResponseDTO reservation)
        {
            if (_reservationService.GetAll().Any(r => r.Id == id) == false) 
            {
                return BadRequest("Reservation not found");
            }
            if (reservation == null)
            {
                return BadRequest("Reservation is null");
            }
            
            if(_userService.GetById(reservation.UserId) == null)
            {
                return BadRequest("User not found");
            }
            
            if(_artworkService.GetAll().FirstOrDefault(a => a.Id == reservation.ArtworkId) == null)
            {
                return BadRequest("Artwork not found");
            }
            
            if(_packageService.GetAll().FirstOrDefault(p => p.Id == reservation.PackageId) == null)
            {
                return BadRequest("Package not found");
            }
            _reservationService.Update(_mapper.Map<Reservation>(reservation));
            return Ok();
        }

        // [HttpDelete("delete-reservation/{id}")]
        // public IActionResult RemoveReservation(int id)
        // {
        //     var reservation = _reservationService.GetAll()
        //         .FirstOrDefault(r => r.Id == id);
        //     if (reservation == null)
        //     {
        //        return BadRequest("Reservation not found");
        //     }
        //     _reservationService.Remove(reservation);
        //     return Ok();
        // }

        [HttpPost("make-reservation")]
        public IActionResult MakeReservation([FromBody] ReservationRequestDTO reservation)
        {
            if (_reservationService.MakeReservation(reservation))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
