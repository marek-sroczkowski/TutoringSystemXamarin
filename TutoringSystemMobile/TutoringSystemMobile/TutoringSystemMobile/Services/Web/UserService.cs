using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Errors;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly:Dependency(typeof(UserService))]
namespace TutoringSystemMobile.Services.Web
{
    public class UserService : IUserService
    {
        public async Task<bool> ActivateUserByTokenAsync(string activationToken)
        {
            string token = await SecureStorage.GetAsync("token");
            string url = AppSettingsManager.Settings["BaseApiUrl"] + "account/activate";
            var response = await url
                .SetQueryParam("token", activationToken)
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PostAsync();

            return response.StatusCode == 200;
        }

        public Task<ICollection<WrongPasswordStatus>> ChangePasswordAsync(PasswordDto passwordModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeactivateUserAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Role> GetUserRole()
        {
            string token = await SecureStorage.GetAsync("token");
            string url = AppSettingsManager.Settings["BaseApiUrl"] + "account/role";
            var response = await url
                .WithOAuthBearerToken(token)
                .GetAsync();

            var userRole = await GetUserRole(response);

            return userRole;
        }

        public Task<bool> RegisterStudentAsync(RegisterStudentDto student)
        {
            throw new NotImplementedException();
        }

        public async Task<RegisterErrorTypes> RegisterTutorAsync(RegisterTutorDto tutor)
        {
            string url = AppSettingsManager.Settings["BaseApiUrl"] + "account/register/tutor";
            var response = await url
                .AllowAnyHttpStatus()
                .PostJsonAsync(tutor);

            var content = await response.GetJsonAsync<RegisterError>();

            return content is null ? null : content.Errors;
        }

        public async Task<bool> SendNewActivationTokenAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            string url = AppSettingsManager.Settings["BaseApiUrl"] + "account/newCode";
            var response = await url
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PostAsync();

            return response.StatusCode == 200;
        }

        public async Task<LoginStatus> TryLoginAsync(LoginUserDto userModel)
        {
            string url = AppSettingsManager.Settings["BaseApiUrl"] + "account/login";
            var response = await url
                .PostJsonAsync(userModel);

            var loginStatus = await GetLoginResultAsync(response);

            if (loginStatus != LoginStatus.InvalidUsernameOrPassword)
                await SecureStorage.SetAsync("token", response.Headers.FirstOrDefault("authorization"));

            return loginStatus;
        }

        private async Task<LoginStatus> GetLoginResultAsync(IFlurlResponse response)
        {
            var loginStatus = await response.GetStringAsync();

            return (LoginStatus)Enum.Parse(typeof(LoginStatus), loginStatus.Trim('\"'));
        }

        private async Task<Role> GetUserRole(IFlurlResponse response)
        {
            var role = await response.GetStringAsync();

            return (Role)Enum.Parse(typeof(Role), role.Trim('\"'));
        }
    }
}
