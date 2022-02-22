using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Dtos.Reservation
{
    public class NewStudentRecurringReservationDto : NewStudentSingleReservationDto
    {
        public ReservationFrequency Frequency { get; set; }
    }
}
