using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.AccountDtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
    }
}
