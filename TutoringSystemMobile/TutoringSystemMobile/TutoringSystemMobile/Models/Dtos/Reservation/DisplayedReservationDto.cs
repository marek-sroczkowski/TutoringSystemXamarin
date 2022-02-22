using System;
using System.IO;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Dtos.Images;
using TutoringSystemMobile.Services.SQLite;
using Xamarin.Forms;

namespace TutoringSystemMobile.Models.Dtos.Reservation
{
    public class DisplayedReservationDto
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
        public ImageSource Image { get; set; }

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
            Duration = $"{reservation.Duration} min";
            SetPlace(reservation.Place);
            var image = SQLiteManager.Instance.Get<UserImageDto>(reservation.StudentId);
            Image = image is null
                ? ResourceConstans.DefaultStudentPicture
                : ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(image.ProfilePictureBase64)));
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