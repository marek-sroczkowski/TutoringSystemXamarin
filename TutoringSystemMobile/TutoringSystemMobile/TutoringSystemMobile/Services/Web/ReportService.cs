using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Report;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Constans;

[assembly: Dependency(typeof(ReportService))]
namespace TutoringSystemMobile.Services.Web
{
    public class ReportService : IReportService
    {
        private readonly string baseUrl;

        public ReportService()
        {
            baseUrl = Settings.BaseApiUrl + ServicesConstans.Report;
        }

        public async Task<GeneralReportDto> GetGeneralReportAsync(ReportParameters parameters)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(ServicesConstans.Summary)
                .SetQueryParams(parameters)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<GeneralReportDto>() : new GeneralReportDto();
        }

        public async Task<IEnumerable<GeneralTimedReportDto>> GetGeneralTimedReport(ReportParameters parameters)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(ServicesConstans.Summary, ServicesConstans.Timed)
                .SetQueryParams(parameters)
                .GetAsync();

            return response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<GeneralTimedReportDto>>()
                : new List<GeneralTimedReportDto>();
        }

        public async Task<IEnumerable<StudentReportDto>> GetStudentsReportAsync(ReportParameters parameters)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(ServicesConstans.Summary, ServicesConstans.Students)
                .SetQueryParams(parameters)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<StudentReportDto>>() : new List<StudentReportDto>();
        }

        public async Task<IEnumerable<SubjectCategoryReportDto>> GetSubjectCategoryReportAsync(ReportParameters parameters)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(ServicesConstans.Summary, ServicesConstans.Subjects, ServicesConstans.Categories)
                .SetQueryParams(parameters)
                .GetAsync();

            return response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<SubjectCategoryReportDto>>()
                : new List<SubjectCategoryReportDto>();
        }

        public async Task<IEnumerable<SubjectReportDto>> GetSubjectReportAsync(ReportParameters parameters)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(ServicesConstans.Summary, ServicesConstans.Subjects)
                .SetQueryParams(parameters)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<SubjectReportDto>>() : new List<SubjectReportDto>();
        }

        public async Task<IEnumerable<PlaceReportDto>> GetPlaceReportAsync(ReportParameters parameters)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(ServicesConstans.Summary, ServicesConstans.Places)
                .SetQueryParams(parameters)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<PlaceReportDto>>() : new List<PlaceReportDto>();
        }
    }
}