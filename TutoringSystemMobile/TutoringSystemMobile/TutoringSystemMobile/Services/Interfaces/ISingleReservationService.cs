using System.Threading.Tasks;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReservationDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface ISingleReservationService
    {
        Task<ReservationsCollectionDto> GetReservationsByStudentAsync(ReservationParameters parameters);
        Task<ReservationsCollectionDto> GetReservationsByTutorAsync(ReservationParameters parameters);
        Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId);
        Task<long> AddReservationByStudentAsync(NewStudentSingleReservationDto newReservation);
        Task<long> AddReservationByTutorAsync(NewTutorSingleReservationDto newReservation);
        Task<bool> UpdateTutorReservationAsync(UpdatedTutorReservationDto updatedReservation);
        Task<bool> DeleteReservationAsync(long reservationId);
        Task<bool> UpdateStudentReservationAsync(UpdatedStudentReservationDto updatedReservation);
    }
}