namespace TutoringSystemMobile.Models.Dtos.Authentication
{
    public class TokenRefreshRequestDto
    {
        public string RefreshToken { get; set; }
        public string DeviceIdentificator { get; set; }

        public TokenRefreshRequestDto()
        {
        }

        public TokenRefreshRequestDto(string refreshToken, string deviceIdentificator)
        {
            RefreshToken = refreshToken;
            DeviceIdentificator = deviceIdentificator;
        }
    }
}