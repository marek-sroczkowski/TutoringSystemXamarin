using System;
using System.IO;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Dtos.Images;
using TutoringSystemMobile.Services.SQLite;
using Xamarin.Forms;

namespace TutoringSystemMobile.Models.Dtos.Student
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
            var image = SQLiteManager.Instance.Get<UserImageDto>(student.Id);
            Image = image is null
                ? ResourceConstans.DefaultStudentPicture
                : ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(image.ProfilePictureBase64)));
        }
    }
}
