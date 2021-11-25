namespace TutoringSystemMobile.Models.Helpers
{
    public class OrderSortingRadioButtonsActivity
    {
        public bool IsSortingByNameAsc { get; set; }
        public bool IsSortingByNameDesc { get; set; }
        public bool IsSortingByPriceAsc { get; set; }
        public bool IsSortingByPriceDesc { get; set; }
        public bool IsSortingByDeadlineAsc { get; set; }
        public bool IsSortingByDeadlineDesc { get; set; }
        public bool IsSortingByCreatedDateAsc { get; set; }
        public bool IsSortingByCreatedDateDesc { get; set; }

        public OrderSortingRadioButtonsActivity()
        {
        }

        public OrderSortingRadioButtonsActivity(bool isSortingByNameAsc, bool isSortingByNameDesc, bool isSortingByPriceAsc, bool isSortingByPriceDesc, bool isSortingByDeadlineAsc, bool isSortingByDeadlineDesc, bool isSortingByCreatedDateAsc, bool isSortingByCreatedDateDesc)
        {
            IsSortingByNameAsc = isSortingByNameAsc;
            IsSortingByNameDesc = isSortingByNameDesc;
            IsSortingByPriceAsc = isSortingByPriceAsc;
            IsSortingByPriceDesc = isSortingByPriceDesc;
            IsSortingByDeadlineAsc = isSortingByDeadlineAsc;
            IsSortingByDeadlineDesc = isSortingByDeadlineDesc;
            IsSortingByCreatedDateAsc = isSortingByCreatedDateAsc;
            IsSortingByCreatedDateDesc = isSortingByCreatedDateDesc;
        }
    }
}
