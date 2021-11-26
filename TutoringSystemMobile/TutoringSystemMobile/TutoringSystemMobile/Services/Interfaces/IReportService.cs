using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReportDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IReportService
    {
        Task<GeneralReportDto> GetGeneralReportAsync(ReportParameters parameters);
        Task<IEnumerable<StudentReportDto>> GetStudentsReportAsync(ReportParameters parameters);
        Task<IEnumerable<SubjectCategoryReportDto>> GetSubjectCategoryReportAsync(ReportParameters parameters);
        Task<IEnumerable<SubjectReportDto>> GetSubjectReportAsync(ReportParameters parameters);
        Task<IEnumerable<PlaceReportDto>> GetPlaceReportAsync(ReportParameters parameters);
    }
}
