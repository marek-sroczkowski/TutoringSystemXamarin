namespace TutoringSystemMobile.Models.Helpers
{
    public class ReportSortingRadioButtonsActivity
    {
        public bool IsSortingByProfitAsc { get; set; }
        public bool IsSortingByProfitDesc { get; set; }
        public bool IsSortingByHoursAsc { get; set; }
        public bool IsSortingByHoursDesc { get; set; }
        public bool IsSortingByReservationsCountAsc { get; set; }
        public bool IsSortingByReservationsCountDesc { get; set; }

        public ReportSortingRadioButtonsActivity()
        {
        }

        public ReportSortingRadioButtonsActivity(bool isSortingByProfitAsc, bool isSortingByProfitDesc, bool isSortingByHoursAsc, bool isSortingByHoursDesc, bool isSortingByReservationsCountAsc, bool isSortingByReservationsCountDesc)
        {
            IsSortingByProfitAsc = isSortingByProfitAsc;
            IsSortingByProfitDesc = isSortingByProfitDesc;
            IsSortingByHoursAsc = isSortingByHoursAsc;
            IsSortingByHoursDesc = isSortingByHoursDesc;
            IsSortingByReservationsCountAsc = isSortingByReservationsCountAsc;
            IsSortingByReservationsCountDesc = isSortingByReservationsCountDesc;
        }
    }
}
