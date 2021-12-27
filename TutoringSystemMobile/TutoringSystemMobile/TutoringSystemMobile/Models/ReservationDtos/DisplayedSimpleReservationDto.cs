using System;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReservationDtos
{
    public class DisplayedSimpleReservationDto
    {
        private readonly DateTime startTime;
        private readonly DateTime endTime;

        public long Id { get; set; }
        public string Cost { get; set; }
        public string LessonTime { get; set; }
        public string Duration { get; set; }
        public string Place { get; set; }
        public string SubjectName { get; set; }
        public string Tutor { get; set; }
        public string Student { get; set; }

        public DisplayedSimpleReservationDto()
        {
        }

        public DisplayedSimpleReservationDto(ReservationDto reservation)
        {
            startTime = reservation.StartTime;
            endTime = reservation.StartTime.AddMinutes(reservation.Duration);
            Id = reservation.Id;
            Cost = $"{reservation.Cost} {GeneralConstans.Pln}";
            SubjectName = reservation.SubjectName;
            Tutor = reservation.Tutor;
            Student = reservation.Student;
            LessonTime = $"{startTime.ToShortTimeString()} - {endTime.ToShortTimeString()}";
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
