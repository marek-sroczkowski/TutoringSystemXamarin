using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReservationDtos
{
    public class NewStudentRecurringReservationDto : NewStudentSingleReservationDto
    {
        public ReservationFrequency Frequency { get; set; }
    }
}
