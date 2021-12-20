namespace TutoringSystemMobile.Models.ImagesDtos
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