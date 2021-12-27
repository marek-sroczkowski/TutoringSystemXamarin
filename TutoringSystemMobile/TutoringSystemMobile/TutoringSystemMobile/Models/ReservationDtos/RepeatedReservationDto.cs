using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReservationDtos
{
    public class RepeatedReservationDto
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastAddedDate { get; set; }
        public DateTime NextAddedDate { get; set; }
        public ReservationFrequency Frequency { get; set; }
    }
}