using TutoringSystemMobile.Models.Dtos.Account;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Dtos.Authentication
{
    public class AuthenticationResposneDto
    {
        public AuthenticationStatus Status { get; set; }
        public UserDto User { get; set; }
        public TokenDto JwtToken { get; set; }
        public TokenDto RefreshToken { get; set; }

        public AuthenticationResposneDto()
        {
        }

        public AuthenticationResposneDto(AuthenticationStatus loginStatus, UserDto user)
        {
            Status = loginStatus;
            User = user;
        }
    }
}