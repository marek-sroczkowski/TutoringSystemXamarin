using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReservationDtos
{
    public class ReservationDto
    {
        public long Id { get; set; }
        public double Cost { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public ReservationPlace Place { get; set; }
        public ReservationType Type { get; set; }
        public ReservationFrequency? Frequency { get; set; }
        public long? RepeatedReservationId { get; set; }
        public string SubjectName { get; set; }
        public long TutorId { get; set; }
        public string Tutor { get; set; }
        public long StudentId { get; set; }
        public string Student { get; set; }

        public ReservationDto()
        {
        }

        public ReservationDto(ReservationDto reservation)
        {
            Id = reservation.Id;
            Cost = reservation.Cost;
            Duration = reservation.Duration;
            Place = reservation.Place;
            Type = reservation.Type;
            SubjectName = reservation.SubjectName;
            TutorId = reservation.TutorId;
            Tutor = reservation.Tutor;
            StudentId = reservation.StudentId;
            Student = reservation.Student;
            Frequency = reservation.Frequency;
            RepeatedReservationId = reservation.RepeatedReservationId;
        }

        public ReservationDto(RepeatedReservationDto reservation)
        {
            Id = reservation.ExampleReservationId;
            Duration = reservation.Duration;
            Place = reservation.Place;
            Type = ReservationType.Recurring;
            SubjectName = reservation.SubjectName;
            TutorId = reservation.TutorId;
            Tutor = reservation.Tutor;
            StudentId = reservation.StudentId;
            Student = reservation.Student;
            Frequency = reservation.Frequency;
            RepeatedReservationId = reservation.Id;
            StartTime = reservation.StartTime;
        }
    }
}