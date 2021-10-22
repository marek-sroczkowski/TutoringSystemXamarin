using TutoringSystemMobile.Models.StudentDtos;

namespace TutoringSystemMobile.Models.ReportDtos
{
    public class StudentSummaryDto
    {
        public StudentDto Student { get; set; }
        public double Hours { get; set; }
        public double Profit { get; set; }
    }
}
