using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReservationDtos
{
    public class NewTutorSingleReservationDto
    {
        public double? Cost { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public ReservationPlace Place { get; set; }

        public long SubjectId { get; set; }
        public long StudentId { get; set; }

        public NewTutorSingleReservationDto()
        {
        }

        public NewTutorSingleReservationDto(double? cost, DateTime startTime, int duration, string description, ReservationPlace place, long subjectId, long studentId)
        {
            Cost = cost;
            StartTime = startTime;
            Duration = duration;
            Description = description;
            Place = place;
            SubjectId = subjectId;
            StudentId = studentId;
        }
    }
}
