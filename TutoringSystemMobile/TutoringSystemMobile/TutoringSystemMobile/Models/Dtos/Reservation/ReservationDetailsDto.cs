using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Dtos.Reservation
{
    public class ReservationDetailsDto
    {
        public long Id { get; set; }
        public double Cost { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public ReservationPlace Place { get; set; }
        public ReservationType Type { get; set; }
        public ReservationFrequency? Frequency { get; set; }
        public long SubjectId { get; set; }
        public string SubjectName { get; set; }
        public long TutorId { get; set; }
        public string Tutor { get; set; }
        public long StudentId { get; set; }
        public string Student { get; set; }
        public bool IsPaid { get; set; }
    }
}