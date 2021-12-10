using System;
using System.IO;
using TutoringSystemMobile.Constans;
using Xamarin.Forms;

namespace TutoringSystemMobile.Models.TutorDtos
{
    public class DisplayedTutorDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string TutorName { get; set; }
        public string HourlRate { get; set; }
        public ImageSource Image { get; set; }

        public DisplayedTutorDto()
        {
        }

        public DisplayedTutorDto(TutorDto tutor)
        {
            Id = tutor.Id;
            Username = tutor.Username;
            TutorName = $"{tutor.FirstName} {tutor.LastName}";
            HourlRate = $"{tutor.HourlRate} {GeneralConstans.PlnPerHours}";
            Image = string.IsNullOrEmpty(tutor.ProfilePictureBase64)
                ? ResourceConstans.DefaultTutorPicture
                : ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(tutor.ProfilePictureBase64)));
        }
    }
}
