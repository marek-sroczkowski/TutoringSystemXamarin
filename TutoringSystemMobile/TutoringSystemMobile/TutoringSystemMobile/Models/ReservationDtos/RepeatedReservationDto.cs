using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReservationDtos
{
    public class RepeatedReservationDto
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public ReservationPlace Place { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastAddedDate { get; set; }
        public DateTime NextAddedDate { get; set; }
        public ReservationFrequency Frequency { get; set; }
        public long StudentId { get; set; }
        public string Student { get; set; }
        public long TutorId { get; set; }
        public string Tutor { get; set; }
        public long SubjectId { get; set; }
        public string SubjectName { get; set; }
        public long ExampleReservationId { get; set; }
    }
}