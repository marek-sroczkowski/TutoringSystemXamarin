using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReservationDtos;
using TutoringSystemMobile.Services.Interfaces;

namespace TutoringSystemMobile.Services.Web
{
    public class SingleReservationService : ISingleReservationService
    {
        public Task<ReservationDto> AddReservationByStudentAsync(NewStudentSingleReservationDto newReservation)
        {
            throw new NotImplementedException();
        }

        public Task<ReservationDto> AddReservationByTutorAsync(NewTutorSingleReservationDto newReservation)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteReservationAsync(long reservationId)
        {
            throw new NotImplementedException();
        }

        public Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ReservationDto>> GetReservationsByStudentAsync(ReservationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ReservationDto>> GetReservationsByTutorAsync(ReservationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStudentReservationAsync(UpdatedStudentReservationDto updatedReservation)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTutorReservationAsync(UpdatedTutorReservationDto updatedReservation)
        {
            throw new NotImplementedException();
        }
    }
}
