using System.Collections.Generic;
using TutoringSystemMobile.Constans;

namespace TutoringSystemMobile.Helpers
{
    public class DateTimeHelper
    {
        public static List<string> GetMonthNames()
        {
            var months = new List<string>
            {
                PickerConstans.January,
                PickerConstans.February,
                PickerConstans.March,
                PickerConstans.April,
                PickerConstans.May,
                PickerConstans.June,
                PickerConstans.July,
                PickerConstans.August,
                PickerConstans.September,
                PickerConstans.October,
                PickerConstans.November,
                PickerConstans.December
            };

            return months;
        }
    }
}