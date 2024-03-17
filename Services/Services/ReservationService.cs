using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Repository.Interface;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Enum;

namespace Services.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IArtworkRepository _artworkRepository;

        public ReservationService(IReservationRepository reservationRepository, IArtworkRepository artworkRepository)
        {
            _reservationRepository = reservationRepository;
            _artworkRepository = artworkRepository;
        }
        public List<Reservation> GetAll()
        {
            return _reservationRepository.GetAll().ToList();
        }

        public void Add(Reservation reservation)
        {
            _reservationRepository.Add(reservation);
        }

        public void Update(Reservation reservation)
        {
            _reservationRepository.Update(reservation);
        }

        public void Remove(Reservation reservation)
        {
            _reservationRepository.Delete(reservation);
        }

        public bool MakeReservation(ReservationRequestDTO reservationDto)
        {
            Artwork? artwork = _artworkRepository.GetById(reservationDto.ArtworkId);
            if (artwork == null || artwork.ArtworkStatus == ArtworkStatus.Sold)
            {
                return false;
            }
            Reservation reservation = new()
            {
                UserId = reservationDto.UserId,
                ArtworkId = reservationDto.ArtworkId,
                Date = DateTime.Now,
                IsDeleted = false,
            };
            _reservationRepository.Add(reservation);
            if (artwork.ArtworkStatus == ArtworkStatus.Available)
            {
                artwork.ArtworkStatus = ArtworkStatus.Reserved;
                _artworkRepository.Update(artwork);
            }
            return true;
        }
    }
}
