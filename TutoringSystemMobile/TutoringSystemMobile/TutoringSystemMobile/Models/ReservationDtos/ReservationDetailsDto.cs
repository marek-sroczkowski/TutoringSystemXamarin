using System;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Models.SubjectDtos;
using TutoringSystemMobile.Models.TutorDtos;

namespace TutoringSystemMobile.Models.ReservationDtos
{
    public class ReservationDetailsDto
    {
        public long Id { get; set; }
        public double Cost { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public ReservationPlace Place { get; set; }

        public SubjectDto Subject { get; set; }
        public TutorDto Tutor { get; set; }
        public StudentDto Student { get; set; }
    }
}
