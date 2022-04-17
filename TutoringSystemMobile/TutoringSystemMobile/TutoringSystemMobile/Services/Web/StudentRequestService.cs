using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Dtos.StudentRequest;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Constans;

[assembly: Dependency(typeof(StudentRequestService))]
namespace TutoringSystemMobile.Services.Web
{
    public class StudentRequestService : IStudentRequestService
    {
        private readonly string baseUrl;

        public StudentRequestService()
        {
            baseUrl = $"{Settings.BaseApiUrl}{ServicesConstans.Student}/{ServicesConstans.Request}";
        }

        public async Task<AddTutorToStudentStatus> AddRequestAsync(long tutorId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(tutorId)
                .PostAsync();

            return response.StatusCode == 200 ? await response.GetAddedTutorStatusAsync() : AddTutorToStudentStatus.InternalError;
        }

        public async Task<bool> DeclineRequest(long requestId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(ServicesConstans.Decline, requestId)
                .PutAsync();

            return response.StatusCode == 200;
        }

        public async Task<IEnumerable<StudentRequestDto>> GetRequestsByStudentId()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.ByStudent)
                .GetAsync();

            return response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<StudentRequestDto>>()
                : new List<StudentRequestDto>();
        }

        public async Task<IEnumerable<StudentRequestDto>> GetRequestsByTutorId()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.ByTutor)
                .GetAsync();

            return response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<StudentRequestDto>>()
                : new List<StudentRequestDto>();
        }
    }
}