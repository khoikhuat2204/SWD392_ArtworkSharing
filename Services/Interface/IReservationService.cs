using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;

namespace Services.Interface
{
    public interface IReservationService
    {
        public List<Reservation> GetAll();

        public void Add(Reservation reservation);

        public void Update(Reservation reservationt);

        public void Remove(Reservation reservation);

        public bool MakeReservation(ReservationRequestDTO reservationDto);
    }
}
