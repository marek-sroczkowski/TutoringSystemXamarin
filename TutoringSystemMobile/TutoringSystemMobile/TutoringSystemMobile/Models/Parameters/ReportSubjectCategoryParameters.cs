using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Parameters
{
    public class ReportSubjectCategoryParameters
    {
        public SubjectCategory SubjectCategory { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
