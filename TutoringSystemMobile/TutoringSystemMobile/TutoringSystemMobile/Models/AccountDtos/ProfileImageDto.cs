namespace TutoringSystemMobile.Models.AccountDtos
{
    public class ProfileImageDto
    {
        public string ProfilePictureBase64 { get; set; }

        public ProfileImageDto()
        {
        }

        public ProfileImageDto(string profilePictureBase64)
        {
            ProfilePictureBase64 = profilePictureBase64;
        }
    }
}
