using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.Reservation;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IRepeatedReservationService
    {
        Task<IEnumerable<RepeatedReservationDto>> GetReservationsByStudentAsync();
        Task<IEnumerable<RepeatedReservationDto>> GetReservationsByTutorAsync();
    }
}
