using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Pagination;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Student;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Constans;

[assembly: Dependency(typeof(StudentService))]
namespace TutoringSystemMobile.Services.Web
{
    public class StudentService : IStudentService
    {
        private readonly string baseUrl;

        public StudentService()
        {
            baseUrl = Settings.BaseApiUrl + ServicesConstans.Student;
        }

        public async Task<AddStudentToTutorStatus> AddStudentToTutorAsync(NewExistingStudentDto student)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .PostJsonAsync(student);

            return response.StatusCode == 200 ? await response.GetAddedStudentStatusAsync() : AddStudentToTutorStatus.InternalError;
        }

        public async Task<StudentDetailsDto> GetStudentByIdAsync(long studentId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(studentId)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<StudentDetailsDto>() : new StudentDetailsDto();
        }

        public async Task<StudentsCollectionDto> GetStudentsByParamsAsync(SearchedUserParameters parameters)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.All)
                .SetQueryParams(parameters)
                .GetAsync();

            return await GetStudentsAsync(response);
        }

        public async Task<IEnumerable<StudentDto>> GetStudentsAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<StudentDto>>() : new List<StudentDto>();
        }

        public async Task<bool> RemoveAllStudentsAsync()
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.All)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<bool> RemoveStudentAsync(long studentId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(studentId)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<bool> UpdateStudentAsync(UpdatedStudentDto student)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .PutJsonAsync(student);

            return response.StatusCode == 204;
        }

        private static async Task<StudentsCollectionDto> GetStudentsAsync(IFlurlResponse response)
        {
            var tutors = response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<StudentSimpleDto>>()
                : new List<StudentSimpleDto>();

            var pagination = response.StatusCode == 200
                ? JsonConvert.DeserializeObject<PaginationMetadata>(response.Headers.FirstOrDefault("X-Pagination"))
                : new PaginationMetadata();

            return new StudentsCollectionDto { Students = tutors, Pagination = pagination };
        }
    }
}