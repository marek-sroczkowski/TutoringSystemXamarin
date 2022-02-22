using System;

namespace TutoringSystemMobile.Models.Dtos.Report
{
    public class GeneralTimedReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GeneralReportDto Report { get; set; }

        public GeneralTimedReportDto()
        {
        }

        public GeneralTimedReportDto(DateTime startDate, DateTime endDate, GeneralReportDto report)
        {
            StartDate = startDate;
            EndDate = endDate;
            Report = report;
        }
    }
}
