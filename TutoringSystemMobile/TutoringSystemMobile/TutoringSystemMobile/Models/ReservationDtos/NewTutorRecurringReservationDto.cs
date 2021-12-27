using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReservationDtos
{
    public class NewTutorRecurringReservationDto : NewTutorSingleReservationDto
    {
        public ReservationFrequency Frequency { get; set; }

        public NewTutorRecurringReservationDto()
        {
        }

        public NewTutorRecurringReservationDto(double? cost, DateTime startTime, int duration, string description, ReservationPlace place, long subjectId, long studentId, ReservationFrequency frequency)
            : base(cost, startTime, duration, description, place, subjectId, studentId)
        {
            Frequency = frequency;
        }
    }
}