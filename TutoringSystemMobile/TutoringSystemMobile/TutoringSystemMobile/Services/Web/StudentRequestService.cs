using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Dtos.StudentRequest;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(StudentRequestService))]
namespace TutoringSystemMobile.Services.Web
{
    public class StudentRequestService : IStudentRequestService
    {
        private readonly string baseUrl;

        public StudentRequestService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "student/request";
        }

        public async Task<AddTutorToStudentStatus> AddRequestAsync(long tutorId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegment(tutorId)
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PostAsync();

            return response.StatusCode == 200 ?
                await GetAddedStatusAsync(response) :
                AddTutorToStudentStatus.InternalError;
        }

        public async Task<bool> DeclineRequest(long requestId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegments("decline", requestId)
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PutAsync();

            return response.StatusCode == 200;
        }

        public async Task<IEnumerable<StudentRequestDto>> GetRequestsByStudentId()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("byStudent")
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ?
                await response.GetJsonAsync<IEnumerable<StudentRequestDto>>() :
                new List<StudentRequestDto>();
        }

        public async Task<IEnumerable<StudentRequestDto>> GetRequestsByTutorId()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("byTutor")
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ?
                await response.GetJsonAsync<IEnumerable<StudentRequestDto>>() :
                new List<StudentRequestDto>();
        }

        private async Task<AddTutorToStudentStatus> GetAddedStatusAsync(IFlurlResponse response)
        {
            var status = await response.GetStringAsync();

            return (AddTutorToStudentStatus)Enum.Parse(typeof(AddTutorToStudentStatus), status.Trim('\"'));
        }
    }
}
