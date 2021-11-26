﻿using System.Collections.Generic;

namespace TutoringSystemMobile.Models.ReportDtos
{
    public class GeneralReportDto
    {
        public IEnumerable<StudentReportDto> StudentSummary { get; set; }
        public double TutoringProfit{ get; set; }
        public double OrderProfit { get; set; }
        public double TotalProfit { get; set; }
        public double TotalHours { get; set; }
    }
}
