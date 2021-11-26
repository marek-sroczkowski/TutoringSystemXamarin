using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReportDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(ReportService))]
namespace TutoringSystemMobile.Services.Web
{
    public class ReportService : IReportService
    {
        private readonly string baseUrl;

        public ReportService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "report";
        }

        public async Task<GeneralReportDto> GetGeneralReportAsync(ReportParameters parameters)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegment("summary")
                .SetQueryParams(parameters)
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<GeneralReportDto>() : new GeneralReportDto();
        }

        public async Task<IEnumerable<StudentReportDto>> GetStudentsReportAsync(ReportParameters parameters)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegments("summary", "students")
                .SetQueryParams(parameters)
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<StudentReportDto>>() : new List<StudentReportDto>();
        }

        public async Task<IEnumerable<SubjectCategoryReportDto>> GetSubjectCategoryReportAsync(ReportParameters parameters)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegments("summary", "subjects", "categories")
                .SetQueryParams(parameters)
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<SubjectCategoryReportDto>>() : new List<SubjectCategoryReportDto>();
        }

        public async Task<IEnumerable<SubjectReportDto>> GetSubjectReportAsync(ReportParameters parameters)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegments("summary", "subjects")
                .SetQueryParams(parameters)
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<SubjectReportDto>>() : new List<SubjectReportDto>();
        }

        public async Task<IEnumerable<PlaceReportDto>> GetPlaceReportAsync(ReportParameters parameters)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AppendPathSegments("summary", "places")
                .SetQueryParams(parameters)
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<PlaceReportDto>>() : new List<PlaceReportDto>();
        }
    }
}
