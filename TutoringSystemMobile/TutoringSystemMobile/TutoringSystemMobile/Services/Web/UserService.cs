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
        private readonly string baseUrl;

        public UserService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "account";
        }

        public async Task<bool> ActivateUserByTokenAsync(string activationToken)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegment("activate")
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
            var response = await baseUrl
                .AppendPathSegment("role")
                .WithOAuthBearerToken(token)
                .GetAsync();

            var userRole = await GetUserRole(response);

            return userRole;
        }

        public async Task<RegisterErrorTypes> RegisterStudentAsync(RegisterStudentDto student)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegments("register", "student")
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PostJsonAsync(student);

            var content = await response.GetJsonAsync<RegisterError>();

            return content?.Errors;
        }

        public async Task<RegisterErrorTypes> RegisterTutorAsync(RegisterTutorDto tutor)
        {
            var response = await baseUrl
                .AppendPathSegments("register", "tutor")
                .AllowAnyHttpStatus()
                .PostJsonAsync(tutor);

            var content = await response.GetJsonAsync<RegisterError>();

            return content?.Errors;
        }

        public async Task<bool> SendNewActivationTokenAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegment("newCode")
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PostAsync();

            return response.StatusCode == 200;
        }

        public async Task<LoginStatus> TryLoginAsync(LoginUserDto userModel)
        {
            var response = await baseUrl
                .AppendPathSegment("login")
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

        public async Task<bool> UpdateGeneralUserInfoAsync(UpdatedUserDto updatedUser)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PutJsonAsync(updatedUser);

            return response.StatusCode == 204;
        }

        public async Task<ShortUserDto> GetGeneralUserInfoAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<ShortUserDto>() : new ShortUserDto();
        }
    }
}