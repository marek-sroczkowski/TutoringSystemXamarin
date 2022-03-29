using System.Collections.ObjectModel;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Helpers
{
    public class ReservationHelper
    {
        public static ReservationPlace GetPlace(string place)
        {
            return place switch
            {
                PickerConstans.AtTutor => ReservationPlace.AtTutor,
                PickerConstans.AtStudent => ReservationPlace.AtStudent,
                PickerConstans.Online => ReservationPlace.Online,
                _ => ReservationPlace.AtTutor
            };
        }

        public static ReservationFrequency GetFrequency(string frequency)
        {
            return frequency switch
            {
                PickerConstans.WeeklyReservation => ReservationFrequency.Weekly,
                PickerConstans.OnceTwoWeeksReservation => ReservationFrequency.OnceTwoWeeks,
                PickerConstans.MonthlyReservation => ReservationFrequency.Monthly,
                _ => ReservationFrequency.Weekly
            };
        }

        public static ObservableCollection<string> GetFrequencies()
        {
            return new ObservableCollection<string>
            {
                PickerConstans.WeeklyReservation,
                PickerConstans.OnceTwoWeeksReservation,
                PickerConstans.MonthlyReservation
            };
        }

        public static ObservableCollection<string> GetDurations()
        {
            return new ObservableCollection<string>
            {
                "30", "45", "60", "90", "120", "135", "150", "180", "210", "225", "240"
            };
        }
    }
}