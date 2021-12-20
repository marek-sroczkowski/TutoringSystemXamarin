using SQLite;

namespace TutoringSystemMobile.Models.ImagesDtos
{
    public class UserImageDto
    {
        [PrimaryKey]
        public long UserId { get; set; }
        public string ProfilePictureFirebaseUrl { get; set; }
        public string ProfilePictureBase64 { get; set; }

        public UserImageDto()
        {
        }

        public UserImageDto(ProfileImageDetailsDto image)
        {
            UserId = image.UserId;
            ProfilePictureFirebaseUrl = image.ProfilePictureFirebaseUrl;
        }
    }
}