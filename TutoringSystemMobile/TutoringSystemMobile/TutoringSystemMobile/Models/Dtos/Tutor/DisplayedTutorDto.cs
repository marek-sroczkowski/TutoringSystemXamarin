using System;
using System.IO;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Dtos.Images;
using TutoringSystemMobile.Services.SQLite;
using Xamarin.Forms;

namespace TutoringSystemMobile.Models.Dtos.Tutor
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
            SetPicture(tutor);
        }

        private void SetPicture(TutorDto tutor)
        {
            var image = SQLiteManager.Instance.Get<UserImageDto>(tutor.Id);
            Image = image is null
                ? SetDefaultPicutore(tutor.FirstName)
                : ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(image.ProfilePictureBase64)));
        }

        private string SetDefaultPicutore(string firstName)
        {
            return firstName.EndsWith('a') && firstName.ToLower() != "kuba"
                ? ResourceConstans.DefaultFemaleTutorPicture
                : ResourceConstans.DefaultMaleTutorPicture;
        }
    }
}