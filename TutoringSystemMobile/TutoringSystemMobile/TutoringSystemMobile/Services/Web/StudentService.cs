using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(StudentService))]
namespace TutoringSystemMobile.Services.Web
{
    public class StudentService : IStudentService
    {
        private readonly string baseUrl;

        public StudentService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "student";
        }

        public async Task<AddStudentToTutorStatus> AddStudentToTutorAsync(NewExistingStudentDto student)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PostJsonAsync(student);

            return await GetAddedStatusAsync(response);
        }

        public async Task<StudentDetailsDto> GetStudentByIdAsync(long studentId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(studentId)
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<StudentDetailsDto>() : new StudentDetailsDto();
        }

        public async Task<IEnumerable<StudentDto>> GetStudentsAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<StudentDto>>() : new List<StudentDto>();
        }

        public async Task<bool> RemoveAllStudentsAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("all")
                .WithOAuthBearerToken(token)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<bool> RemoveStudentAsync(long studentId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(studentId)
                .WithOAuthBearerToken(token)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        private async Task<AddStudentToTutorStatus> GetAddedStatusAsync(IFlurlResponse response)
        {
            var status = await response.GetStringAsync();

            return (AddStudentToTutorStatus)Enum.Parse(typeof(AddStudentToTutorStatus), status.Trim('\"'));
        }
    }
}