using System;

namespace TutoringSystemMobile.Models.Dtos.Report
{
    public class ReportFilteringDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsIncludeZeroProfit { get; set; }
        public bool IsIncludeZeroProfitVisible { get; set; }

        public ReportFilteringDto()
        {
        }

        public ReportFilteringDto(DateTime startDate, DateTime endDate, bool isIncludeZeroProfit = true, bool isIncludeZeroProfitVisible = true)
        {
            StartDate = startDate;
            EndDate = endDate;
            IsIncludeZeroProfit = isIncludeZeroProfit;
            IsIncludeZeroProfitVisible = isIncludeZeroProfitVisible;
        }
    }
}