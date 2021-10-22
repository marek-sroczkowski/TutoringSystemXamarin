using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReportDtos
{
    public class SubjectCategoryReportDto
    {
        public SubjectCategory SubjectCategory { get; set; }
        public double TotalHours { get; set; }
        public double TotalProfit { get; set; }
    }
}
