using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Subject;
using TutoringSystemMobile.Models.Errors;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(SubjectService))]
namespace TutoringSystemMobile.Services.Web
{
    public class SubjectService : ISubjectService
    {
        private readonly string baseUrl;

        public SubjectService()
        {
            baseUrl = Settings.BaseApiUrl + "subject";
        }

        public async Task<long> AddSubjectAsync(NewSubjectDto newSubjectModel)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .PostJsonAsync(newSubjectModel);

            return await GetAddedResultAsync(response);
        }

        public async Task<bool> DeleteSubjectAsync(long subjectId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(subjectId)
                .WithOAuthBearerToken(token)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<SubjectDetailsDto> GetSubjectByIdAsync(long subjectId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(subjectId)
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<SubjectDetailsDto>() : new SubjectDetailsDto();
        }

        public async Task<IEnumerable<SubjectDto>> GetSubjectsAsync()
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<SubjectDto>>() : new List<SubjectDto>();
        }

        public async Task<IEnumerable<SubjectDto>> GetSubjectByTutorId(long tutorId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegments("student", tutorId)
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<SubjectDto>>() : new List<SubjectDto>();
        }

        public async Task<long> UpdateSubjectAsync(UpdatedSubjectDto updatedSubject)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .PutJsonAsync(updatedSubject);

            return await GetUpdatedResultAsync(response);
        }

        private static async Task<long> GetAddedResultAsync(IFlurlResponse response)
        {
            var content = await response.GetJsonAsync<ResponseError<SubjectErrors>>();
            if (content?.Errors?.Name != null)
            {
                return -2;
            }
            else if (response.StatusCode != 201)
            {
                return -1;
            }

            string location = response.Headers.FirstOrDefault("location");

            return location is null ? -1 : location.GetIdByLocation();
        }

        private static async Task<long> GetUpdatedResultAsync(IFlurlResponse response)
        {
            var content = await response.GetJsonAsync<ResponseError<SubjectErrors>>();
            if (content?.Errors?.Name != null)
            {
                return -2;
            }
            else if (response.StatusCode != 204)
            {
                return -1;
            }

            return 1;
        }
    }
}