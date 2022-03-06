using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Dtos.Account
{
    public class LoginResposneDto
    {
        public LoginStatus LoginStatus { get; set; }
        public UserDto User { get; set; }

        public LoginResposneDto()
        {
        }

        public LoginResposneDto(LoginStatus loginStatus, UserDto user)
        {
            LoginStatus = loginStatus;
            User = user;
        }
    }
}