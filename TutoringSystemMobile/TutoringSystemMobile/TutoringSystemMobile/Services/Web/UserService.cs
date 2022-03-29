using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Account;
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
            baseUrl = Settings.BaseApiUrl + "account";
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

        public async Task<PasswordChangeError> ChangePasswordAsync(PasswordDto passwordModel)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegment("password")
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PatchJsonAsync(passwordModel);

            return await GetPasswordChangeError(response);
        }

        public async Task<bool> DeactivateUserAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .DeleteAsync();

            return response.StatusCode == 204;
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

        public async Task<RegisterErrors> RegisterStudentAsync(RegisteredStudentDto student)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegments("register", "student")
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PostJsonAsync(student);

            var content = await response.GetJsonAsync<ResponseError<RegisterErrors>>();

            return content?.Errors;
        }

        public async Task<RegisterErrors> CreateStudentAsync(NewStudentDto student)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegments("create", "student")
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PostJsonAsync(student);

            var content = await response.GetJsonAsync<ResponseError<RegisterErrors>>();

            return content?.Errors;
        }

        public async Task<RegisterErrors> RegisterTutorAsync(RegisteredTutorDto tutor)
        {
            var response = await baseUrl
                .AppendPathSegments("register", "tutor")
                .AllowAnyHttpStatus()
                .PostJsonAsync(tutor);

            var content = await response.GetJsonAsync<ResponseError<RegisterErrors>>();

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

        public async Task<LoginResposneDto> TryLoginAsync(LoginUserDto userModel)
        {
            var response = await baseUrl
                .AppendPathSegment("login")
                .PostJsonAsync(userModel);

            var loginResponse = await response.GetJsonAsync<LoginResposneDto>();

            if (loginResponse.LoginStatus != LoginStatus.InvalidUsernameOrPassword)
                await SecureStorage.SetAsync("token", response.Headers.FirstOrDefault("authorization"));

            return loginResponse;
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

        private async Task<PasswordChangeError> GetPasswordChangeError(IFlurlResponse response)
        {
            if (response.StatusCode != 400)
            {
                return null;
            }

            PasswordChangeError result;
            try
            {
                var content = await response.GetJsonAsync<ResponseError<PasswordChangeError>>();
                result = content.Errors;
            }
            catch (Exception)
            {
                result = new PasswordChangeError { PasswordErrors = new List<string>() { WrongPasswordStatus.InternalError.ToString() } };
            }

            return result;
        }
    }
}