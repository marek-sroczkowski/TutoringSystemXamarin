using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Pagination;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Tutor;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;

[assembly: Dependency(typeof(TutorService))]
namespace TutoringSystemMobile.Services.Web
{
    public class TutorService : ITutorService
    {
        private readonly string baseUrl;

        public TutorService()
        {
            baseUrl = Settings.BaseApiUrl + "tutor";
        }

        public async Task<IEnumerable<TutorDto>> GetTutorsByStudentAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<TutorDto>>() : new List<TutorDto>();
        }

        public async Task<TutorsCollectionDto> GetTutorsByParamsAsync(SearchedUserParameters parameters)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment("all")
                .SetQueryParams(parameters)
                .WithOAuthBearerToken(token)
                .GetAsync();

            IEnumerable<TutorSimpleDto> tutors = response.StatusCode == 200 ?
                await response.GetJsonAsync<IEnumerable<TutorSimpleDto>>() :
                new List<TutorSimpleDto>();

            var pagination = response.StatusCode == 200 ?
                JsonConvert.DeserializeObject<PaginationMetadata>(response.Headers.FirstOrDefault("X-Pagination")) :
                new PaginationMetadata();

            return new TutorsCollectionDto { Tutors = tutors, Pagination = pagination };
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