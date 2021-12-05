using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReportDtos
{
    public class PlaceSummaryDto
    {
        public string PlaceName { get; set; }
        public string ReservationsCount { get; set; }
        public string TotalHours { get; set; }
        public string TotalProfit { get; set; }

        public PlaceSummaryDto()
        {
        }

        public PlaceSummaryDto(ReservationPlace place, int reservationsCount, double totalHours, double totalProfit)
        {
            SetPlaceName(place);
            TotalProfit = $" {totalProfit} zł ";
            ReservationsCount = $" {reservationsCount} ";
            TotalHours = $" {totalHours}h ";
        }

        private void SetPlaceName(ReservationPlace place)
        {
            switch (place)
            {
                case ReservationPlace.AtTutor:
                    PlaceName = PickerConstans.AtTutor;
                    break;
                case ReservationPlace.AtStudent:
                    PlaceName = PickerConstans.AtStudent;
                    break;
                case ReservationPlace.Online:
                    PlaceName = PickerConstans.Online;
                    break;
                default:
                    break;
            }
        }
    }
}