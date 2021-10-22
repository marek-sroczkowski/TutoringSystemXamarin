using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReservationDtos
{
    public class NewTutorRecurringReservationDto : NewTutorSingleReservationDto
    {
        public ReservationFrequency Frequency { get; set; }
    }
}
