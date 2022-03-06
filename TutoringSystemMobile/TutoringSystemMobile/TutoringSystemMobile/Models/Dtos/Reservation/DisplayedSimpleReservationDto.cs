using System;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Dtos.Reservation
{
    public class DisplayedSimpleReservationDto
    {
        public long Id { get; set; }
        public string Cost { get; set; }
        public string LessonTime { get; set; }
        public string Duration { get; set; }
        public string Place { get; set; }
        public string SubjectName { get; set; }
        public string Tutor { get; set; }
        public string Student { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public DisplayedSimpleReservationDto()
        {
        }

        public DisplayedSimpleReservationDto(ReservationDto reservation)
        {
            StartTime = reservation.StartTime;
            EndTime = reservation.StartTime.AddMinutes(reservation.Duration);
            Id = reservation.Id;
            Cost = $"{reservation.Cost} {GeneralConstans.Pln}";
            SubjectName = reservation.SubjectName;
            Tutor = reservation.Tutor;
            Student = reservation.Student;
            LessonTime = $"{StartTime.ToShortTimeString()} - {EndTime.ToShortTimeString()}";
            Duration = $"{reservation.Duration} {PickerConstans.MinutesShort}";
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
