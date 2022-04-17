using Flurl.Http;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Authentication;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Errors;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationService))]
namespace TutoringSystemMobile.Services.Web
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string baseUrl;

        public AuthenticationService()
        {
            baseUrl = Settings.BaseApiUrl + ServicesConstans.Authentication;
        }

        public async Task<AuthenticationResposneDto> AuthenticateAsync(AuthenticationDto authenticationData)
        {
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .PostJsonAsync(authenticationData);

            var authenticationResponse = await response.GetJsonAsync<AuthenticationResposneDto>();

            if (authenticationResponse.Status != AuthenticationStatus.InvalidUsernameOrPassword)
            {
                await AuthenticationHelper.StoreTokensAsync(authenticationResponse);
            }

            return authenticationResponse;
        }

        public async Task<RefreshTokenError> RefreshJwt(TokenRefreshRequestDto jwtRefreshData)
        {
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegments(ServicesConstans.Refresh, ServicesConstans.Jwt)
                .PostJsonAsync(jwtRefreshData);

            if (response.StatusCode == 200)
            {
                await AuthenticationHelper.StoreJwtTokenAsync(await response.GetJsonAsync<TokenDto>());
            }

            var content = await response.GetJsonAsync<ResponseError<RefreshTokenError>>();

            return content?.Errors;
        }

        public async Task<RefreshTokenError> GenerateRefreshToken(TokenRefreshRequestDto refreshTokenData)
        {
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegments(ServicesConstans.Refresh, ServicesConstans.Token)
                .PostJsonAsync(refreshTokenData);

            if (response.StatusCode == 200)
            {
                await AuthenticationHelper.StoreRefreshTokenAsync(await response.GetJsonAsync<TokenDto>());
            }

            var content = await response.GetJsonAsync<ResponseError<RefreshTokenError>>();

            return content?.Errors;
        }
    }
}