using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReservationDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface ISingleReservationService
    {
        Task<ICollection<ReservationDto>> GetReservationsByStudentAsync(ReservationParameters parameters);
        Task<ICollection<ReservationDto>> GetReservationsByTutorAsync(ReservationParameters parameters);
        Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId);
        Task<ReservationDto> AddReservationByStudentAsync(NewStudentSingleReservationDto newReservation);
        Task<ReservationDto> AddReservationByTutorAsync(NewTutorSingleReservationDto newReservation);
        Task<bool> UpdateTutorReservationAsync(UpdatedTutorReservationDto updatedReservation);
        Task<bool> DeleteReservationAsync(long reservationId);
        Task<bool> UpdateStudentReservationAsync(UpdatedStudentReservationDto updatedReservation);
    }
}
