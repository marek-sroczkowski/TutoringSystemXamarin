using System.Threading.Tasks;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReportDtos;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IReportService
    {
        Task<PlaceReportDto> GetPlaceReportAsync(ReportPlaceParameters parameters);
        Task<TutorReportDto> GetReportByTutorAsync(ReportParameters parameters);
        Task<StudentSummaryDto> GetStudentSummaryAsync(long studentId, ReportParameters parameters);
        Task<SubjectCategoryReportDto> GetSubjectCategoryReportAsync(ReportSubjectCategoryParameters parameters);
        Task<SubjectReportDto> GetSubjectReportAsync(long subjectId, ReportParameters parameters);
    }
}
