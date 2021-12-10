using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.TutorDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(TutorService))]
namespace TutoringSystemMobile.Services.Web
{
    public class TutorService : ITutorService
    {
        private readonly string baseUrl;

        public TutorService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "tutor";
        }

        public async Task<bool> AddTutorToStudentAsync(long tutorId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegment(tutorId)
                .WithOAuthBearerToken(token)
                .AllowAnyHttpStatus()
                .PostAsync();

            return response.StatusCode == 200;
        }

        public async Task<IEnumerable<TutorDto>> GetTutorsAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<TutorDto>>() : new List<TutorDto>();
        }

        public async Task<TutorDetailsDto> GetTutorByIdAsync(long tutorId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(tutorId)
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<TutorDetailsDto>() : new TutorDetailsDto();
        }

        public async Task<bool> RemoveAllTutorsAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("all")
                .WithOAuthBearerToken(token)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<bool> RemoveTutorAsync(long tutorId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(tutorId)
                .WithOAuthBearerToken(token)
                .DeleteAsync();

            return response.StatusCode == 204;
        }
    }
}