using System;

namespace TutoringSystemMobile.Models.Parameters
{
    public class ReportParameters
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OrderBy { get; set; }

        public ReportParameters()
        {
        }

        public ReportParameters(DateTime startDate, DateTime endDate, string orderBy)
        {
            StartDate = startDate;
            EndDate = endDate;
            OrderBy = orderBy;
        }
    }
}