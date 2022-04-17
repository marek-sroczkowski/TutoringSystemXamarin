using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Pagination;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Tutor;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Constans;

[assembly: Dependency(typeof(TutorService))]
namespace TutoringSystemMobile.Services.Web
{
    public class TutorService : ITutorService
    {
        private readonly string baseUrl;

        public TutorService()
        {
            baseUrl = Settings.BaseApiUrl + ServicesConstans.Tutor;
        }

        public async Task<IEnumerable<TutorDto>> GetTutorsByStudentAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<TutorDto>>() : new List<TutorDto>();
        }

        public async Task<TutorsCollectionDto> GetTutorsByParamsAsync(SearchedUserParameters parameters)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.All)
                .SetQueryParams(parameters)
                .GetAsync();

            return await GetTutorsAsync(response);
        }

        public async Task<TutorDetailsDto> GetTutorByIdAsync(long tutorId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(tutorId)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<TutorDetailsDto>() : new TutorDetailsDto();
        }

        public async Task<bool> RemoveAllTutorsAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.All)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<bool> RemoveTutorAsync(long tutorId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(tutorId)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        private static async Task<TutorsCollectionDto> GetTutorsAsync(IFlurlResponse response)
        {
            var tutors = response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<TutorSimpleDto>>()
                : new List<TutorSimpleDto>();

            var pagination = response.StatusCode == 200
                ? JsonConvert.DeserializeObject<PaginationMetadata>(response.Headers.FirstOrDefault(ServicesConstans.XPagination))
                : new PaginationMetadata();

            return new TutorsCollectionDto { Tutors = tutors, Pagination = pagination };
        }
    }
}