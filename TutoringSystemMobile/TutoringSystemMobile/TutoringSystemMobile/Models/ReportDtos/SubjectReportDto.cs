using TutoringSystemMobile.Models.SubjectDtos;

namespace TutoringSystemMobile.Models.ReportDtos
{
    public class SubjectReportDto
    {
        public SubjectDto Subject { get; set; }
        public double TotalHours { get; set; }
        public double TotalProfit { get; set; }
    }
}
