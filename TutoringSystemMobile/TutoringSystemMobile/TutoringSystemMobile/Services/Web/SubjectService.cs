using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Subject;
using TutoringSystemMobile.Models.Errors;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(SubjectService))]
namespace TutoringSystemMobile.Services.Web
{
    public class SubjectService : ISubjectService
    {
        private readonly string baseUrl;

        public SubjectService()
        {
            baseUrl = Settings.BaseApiUrl + ServicesConstans.Subject;
        }

        public async Task<long> AddSubjectAsync(NewSubjectDto newSubjectModel)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .PostJsonAsync(newSubjectModel);

            return await GetAddedResultAsync(response);
        }

        public async Task<bool> RemoveSubjectAsync(long subjectId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(subjectId)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<SubjectDetailsDto> GetSubjectByIdAsync(long subjectId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(subjectId)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<SubjectDetailsDto>() : new SubjectDetailsDto();
        }

        public async Task<IEnumerable<SubjectDto>> GetSubjectsAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<SubjectDto>>() : new List<SubjectDto>();
        }

        public async Task<IEnumerable<SubjectDto>> GetSubjectByTutorId(long tutorId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(ServicesConstans.Student, tutorId)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<SubjectDto>>() : new List<SubjectDto>();
        }

        public async Task<long> UpdateSubjectAsync(UpdatedSubjectDto updatedSubject)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
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

            string location = response.Headers.FirstOrDefault(ServicesConstans.Location);

            return location is null ? -1 : location.GetIdByLocation();
        }

        private static async Task<long> GetUpdatedResultAsync(IFlurlResponse response)
        {
            var content = await response.GetJsonAsync<ResponseError<SubjectErrors>>();
            if (content?.Errors?.Name != null)
            {
                return -2;
            }

            return 1;
        }
    }
}