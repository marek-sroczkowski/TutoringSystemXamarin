using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReportDtos
{
    public class PlaceReportDto
    {
        public ReservationPlace Place { get; set; }
        public double TotalHours { get; set; }
        public double TotalProfit { get; set; }
    }
}
