using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReservationDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IRecurringReservationService
    {
        Task<ReservationDto> AddReservationByStudentAsync(NewStudentRecurringReservationDto newReservation);
        Task<ReservationDto> AddReservationByTutorAsync(NewTutorRecurringReservationDto newReservation);
        Task<bool> DeleteReservationAsync(long reservationId);
        Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId);
        Task<ICollection<ReservationDto>> GetReservationsByStudentAsync(ReservationParameters parameters);
        Task<ICollection<ReservationDto>> GetReservationsByTutorAsync(ReservationParameters parameters);
    }
}
