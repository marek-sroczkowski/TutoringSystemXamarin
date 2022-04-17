using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.Authentication;
using TutoringSystemMobile.Models.Errors;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResposneDto> AuthenticateAsync(AuthenticationDto userModel);
        Task<RefreshTokenError> GenerateRefreshToken(TokenRefreshRequestDto refreshTokenData);
        Task<RefreshTokenError> RefreshJwt(TokenRefreshRequestDto jwtRefreshData);
    }
}