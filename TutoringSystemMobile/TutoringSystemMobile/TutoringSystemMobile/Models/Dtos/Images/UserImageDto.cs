using SQLite;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Dtos.Images
{
    public class UserImageDto
    {
        [PrimaryKey]
        public long UserId { get; set; }
        public string ProfilePictureFirebaseUrl { get; set; }
        public string ProfilePictureBase64 { get; set; }
        public Role UserRole { get; set; }

        public UserImageDto()
        {
        }

        public UserImageDto(ProfileImageDetailsDto image, Role role)
        {
            UserId = image.UserId;
            ProfilePictureFirebaseUrl = image.ProfilePictureFirebaseUrl;
            UserRole = role;
        }
    }
}