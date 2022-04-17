using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Reservation;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IRecurringReservationService
    {
        Task<long> AddReservationByStudentAsync(NewStudentRecurringReservationDto newReservation);
        Task<long> AddReservationByTutorAsync(NewTutorRecurringReservationDto newReservation);
        Task<bool> RemoveReservationAsync(long reservationId, RecurringReservationRemovingMode mode);
        Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId);
        Task<ReservationsCollectionDto> GetReservationsByStudentAsync(ReservationParameters parameters);
        Task<ReservationsCollectionDto> GetReservationsByTutorAsync(ReservationParameters parameters);
        Task<bool> UpdateStudentReservationAsync(UpdatedStudentReservationDto updatedReservation);
        Task<bool> UpdateTutorReservationAsync(UpdatedTutorReservationDto updatedReservation);
    }
}