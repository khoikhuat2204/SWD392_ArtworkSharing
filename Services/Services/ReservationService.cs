using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Repository.Interface;

namespace Services.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
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
    }
}
