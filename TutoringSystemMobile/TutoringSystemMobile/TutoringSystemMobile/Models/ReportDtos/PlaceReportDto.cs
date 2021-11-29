using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReportDtos
{
    public class PlaceReportDto : BaseReportDto
    {
        public ReservationPlace Place { get; set; }
    }
}
