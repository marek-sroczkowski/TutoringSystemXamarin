using Flurl.Http;
using System.Threading.Tasks;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Authentication;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationService))]
namespace TutoringSystemMobile.Services.Web
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string baseUrl;

        public AuthenticationService()
        {
            baseUrl = Settings.BaseApiUrl + "authentication";
        }

        public async Task<AuthenticationResposneDto> AuthenticateAsync(AuthenticationDto userModel)
        {
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .PostJsonAsync(userModel);

            var loginResponse = await response.GetJsonAsync<AuthenticationResposneDto>();

            if (loginResponse.LoginStatus != AuthenticationStatus.InvalidUsernameOrPassword)
            {
                await SecureStorage.SetAsync("token", loginResponse.Token.Token);
                await SecureStorage.SetAsync("tokenExpirationDate", loginResponse.Token.ExpirationDate.ToString());
            }

            return loginResponse;
        }
    }
}