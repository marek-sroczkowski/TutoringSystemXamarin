using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReportDtos;
using TutoringSystemMobile.Services.Interfaces;

namespace TutoringSystemMobile.Services.Web
{
    public class ReportService : IReportService
    {
        public Task<PlaceReportDto> GetPlaceReportAsync(ReportPlaceParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<TutorReportDto> GetReportByTutorAsync(ReportParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<StudentSummaryDto> GetStudentSummaryAsync(long studentId, ReportParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<SubjectCategoryReportDto> GetSubjectCategoryReportAsync(ReportSubjectCategoryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<SubjectReportDto> GetSubjectReportAsync(long subjectId, ReportParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
