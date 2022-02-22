namespace TutoringSystemMobile.Models.Dtos.Images
{
    public class ProfileImageDto
    {
        public string ProfilePictureFirebaseUrl { get; set; }

        public ProfileImageDto()
        {
        }

        public ProfileImageDto(string profilePictureFirebaseUrl)
        {
            ProfilePictureFirebaseUrl = profilePictureFirebaseUrl;
        }
    }
}