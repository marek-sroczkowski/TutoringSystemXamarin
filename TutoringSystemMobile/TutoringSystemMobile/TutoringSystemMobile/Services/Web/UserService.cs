using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Account;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Errors;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserService))]
namespace TutoringSystemMobile.Services.Web
{
    public class UserService : IUserService
    {
        private readonly string baseUrl;

        public UserService()
        {
            baseUrl = Settings.BaseApiUrl + ServicesConstans.Account;
        }

        public async Task<bool> ActivateUserByTokenAsync(string activationToken)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Activate)
                .SetQueryParam(ServicesConstans.Token, activationToken)
                .PostAsync();

            return response.StatusCode == 200;
        }

        public async Task<PasswordChangeError> ChangePasswordAsync(PasswordDto passwordModel)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Password)
                .PatchJsonAsync(passwordModel);

            return await GetPasswordChangeError(response);
        }

        public async Task<bool> DeactivateUserAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<Role> GetUserRole()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Role)
                .GetAsync();

            var userRole = await response.GetUserRole();

            return userRole;
        }

        public async Task<RegisterErrors> RegisterStudentAsync(RegisteredStudentDto student)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(ServicesConstans.Register, ServicesConstans.Student)
                .PostJsonAsync(student);

            var content = await response.GetJsonAsync<ResponseError<RegisterErrors>>();

            return content?.Errors;
        }

        public async Task<RegisterErrors> CreateStudentAsync(NewStudentDto student)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(ServicesConstans.Create, ServicesConstans.Student)
                .PostJsonAsync(student);

            var content = await response.GetJsonAsync<ResponseError<RegisterErrors>>();

            return content?.Errors;
        }

        public async Task<RegisterErrors> RegisterTutorAsync(RegisteredTutorDto tutor)
        {
            var response = await baseUrl
                .AppendPathSegments(ServicesConstans.Register, ServicesConstans.Tutor)
                .AllowAnyHttpStatus()
                .PostJsonAsync(tutor);

            var content = await response.GetJsonAsync<ResponseError<RegisterErrors>>();

            return content?.Errors;
        }

        public async Task<bool> SendNewActivationTokenAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.NewCode)
                .PostAsync();

            return response.StatusCode == 200;
        }

        public async Task<bool> UpdateGeneralUserInfoAsync(UpdatedUserDto updatedUser)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .PutJsonAsync(updatedUser);

            return response.StatusCode == 204;
        }

        public async Task<ShortUserDto> GetGeneralUserInfoAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
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