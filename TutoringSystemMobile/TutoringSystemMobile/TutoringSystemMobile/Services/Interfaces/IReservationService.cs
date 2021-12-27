using System.Threading.Tasks;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReservationDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId);
        Task<ReservationsCollectionDto> GetReservationsByStudentAsync(ReservationParameters parameters);
        Task<ReservationsCollectionDto> GetReservationsByTutorAsync(ReservationParameters parameters);
    }
}