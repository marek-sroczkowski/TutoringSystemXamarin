using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Parameters
{
    public class ReportPlaceParameters
    {
        public ReservationPlace Place { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
