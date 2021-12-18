using System;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReservationDtos
{
    public class DisplayedReservationDto
    {
        private DateTime startTime;
        private DateTime endTime;

        public long Id { get; set; }
        public string Cost { get; set; }
        public string LessonTime { get; set; }
        public string Place { get; set; }
        public string SubjectName { get; set; }
        public string Tutor { get; set; }
        public string Student { get; set; }

        public DisplayedReservationDto()
        {
        }

        public DisplayedReservationDto(ReservationDto reservation)
        {
            startTime = reservation.StartTime;
            endTime = reservation.StartTime.AddMinutes(reservation.Duration);
            Id = reservation.Id;
            Cost = $"{reservation.Cost} zł";
            SubjectName = reservation.SubjectName;
            Tutor = reservation.Tutor;
            Student = reservation.Student;
            LessonTime = $"{startTime.ToShortTimeString()} - {endTime.ToShortTimeString()}";
            SetPlace(reservation.Place);
        }

        private void SetPlace(ReservationPlace place)
        {
            Place = place switch
            {
                ReservationPlace.AtTutor => PickerConstans.AtTutor,
                ReservationPlace.AtStudent => PickerConstans.AtStudent,
                ReservationPlace.Online => PickerConstans.Online,
                _ => PickerConstans.LessonOtherPlace
            };
        }
    }
}