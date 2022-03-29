using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Pagination;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Student;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;

[assembly: Dependency(typeof(StudentService))]
namespace TutoringSystemMobile.Services.Web
{
    public class StudentService : IStudentService
    {
        private readonly string baseUrl;

        public StudentService()
        {
            baseUrl = Settings.BaseApiUrl + "student";
        }

        public async Task<AddStudentToTutorStatus> AddStudentToTutorAsync(NewExistingStudentDto student)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PostJsonAsync(student);

            return response.StatusCode == 200 ?
                await GetAddedStatusAsync(response) :
                AddStudentToTutorStatus.InternalError;
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

        public async Task<StudentsCollectionDto> GetStudentsByParamsAsync(SearchedUserParameters parameters)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("all")
                .SetQueryParams(parameters)
                .WithOAuthBearerToken(token)
                .GetAsync();

            IEnumerable<StudentSimpleDto> tutors = response.StatusCode == 200 ?
                await response.GetJsonAsync<IEnumerable<StudentSimpleDto>>() :
                new List<StudentSimpleDto>();

            var pagination = response.StatusCode == 200 ?
                JsonConvert.DeserializeObject<PaginationMetadata>(response.Headers.FirstOrDefault("X-Pagination")) :
                new PaginationMetadata();

            return new StudentsCollectionDto { Students = tutors, Pagination = pagination };
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

        public async Task<bool> UpdateStudentAsync(UpdatedStudentDto student)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .PutJsonAsync(student);

            return response.StatusCode == 204;
        }

        private async Task<AddStudentToTutorStatus> GetAddedStatusAsync(IFlurlResponse response)
        {
            var status = await response.GetStringAsync();

            return (AddStudentToTutorStatus)Enum.Parse(typeof(AddStudentToTutorStatus), status.Trim('\"'));
        }
    }
}