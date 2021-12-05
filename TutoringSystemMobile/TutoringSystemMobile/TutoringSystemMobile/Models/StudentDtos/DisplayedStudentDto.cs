using System;
using System.IO;
using TutoringSystemMobile.Constans;
using Xamarin.Forms;

namespace TutoringSystemMobile.Models.StudentDtos
{
    public class DisplayedStudentDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string StudentName { get; set; }
        public string HourlRate { get; set; }
        public ImageSource Image { get; set; }

        public DisplayedStudentDto()
        {
        }

        public DisplayedStudentDto(StudentDto student)
        {
            Id = student.Id;
            Username = student.Username;
            StudentName = $"{student.FirstName} {student.LastName}";
            HourlRate = $"{student.HourlRate} {GeneralConstans.PlnPerHours}";
            Image = string.IsNullOrEmpty(student.ProfilePictureBase64)
                ? ResourceConstans.DefaultStudentPicture
                : ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(student.ProfilePictureBase64)));
        }
    }
}
