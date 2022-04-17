using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Authentication;
using TutoringSystemMobile.Models.Errors;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.Services.Interceptors
{
    public class TokenRefresher : HttpClientHandler
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IEnvironment environment;

        public TokenRefresher()
        {
            authenticationService = DependencyService.Get<IAuthenticationService>();
            environment = DependencyService.Get<IEnvironment>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await ValidateRefreshTokenAsync();
            await ValidateJwtTokenAsync();

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            return response;
        }

        private async Task ValidateRefreshTokenAsync()
        {
            var refreshToken = await AuthenticationHelper.GetRefreshToken();

            if (refreshToken.ExpirationDate <= DateTime.Now.AddMinutes(-5))
            {
                var refreshData = new TokenRefreshRequestDto(refreshToken.Token, environment.GetDeviceId());
                var errors = await authenticationService.GenerateRefreshToken(refreshData);
                OnTokenRefreshErrorsHandle(errors);
            }
        }

        private async Task ValidateJwtTokenAsync()
        {
            var jwtToken = await AuthenticationHelper.GetJwtToken();

            if (jwtToken.ExpirationDate <= DateTime.Now.AddMinutes(-5))
            {
                var refreshData = new TokenRefreshRequestDto(jwtToken.Token, environment.GetDeviceId());
                var errors = await authenticationService.RefreshJwt(refreshData);
                OnTokenRefreshErrorsHandle(errors);
            }
        }

        private void OnTokenRefreshErrorsHandle(RefreshTokenError error)
        {
            if (error is null)
            {
                return;
            }

            ToastHelper.MakeLongToast(ToastConstans.RefreshTokenError);
            AuthenticationHelper.Logout();
        }
    }
}