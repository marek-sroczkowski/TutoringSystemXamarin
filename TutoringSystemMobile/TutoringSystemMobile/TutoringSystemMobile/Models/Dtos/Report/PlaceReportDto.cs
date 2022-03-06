using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.Dtos.Report
{
    public class PlaceReportDto : BaseReportDto
    {
        public ReservationPlace Place { get; set; }
    }
}
