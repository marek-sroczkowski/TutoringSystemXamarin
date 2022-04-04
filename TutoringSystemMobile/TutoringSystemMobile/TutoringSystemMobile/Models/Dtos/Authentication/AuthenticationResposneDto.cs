using TutoringSystemMobile.Models.Dtos.Account;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Dtos.Authentication
{
    public class AuthenticationResposneDto
    {
        public AuthenticationStatus LoginStatus { get; set; }
        public UserDto User { get; set; }
        public JwtTokenDto Token { get; set; }

        public AuthenticationResposneDto()
        {
        }

        public AuthenticationResposneDto(AuthenticationStatus loginStatus, UserDto user)
        {
            LoginStatus = loginStatus;
            User = user;
        }
    }
}