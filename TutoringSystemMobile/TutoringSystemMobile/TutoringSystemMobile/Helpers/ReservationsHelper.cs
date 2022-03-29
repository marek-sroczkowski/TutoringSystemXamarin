using System;
using System.Collections.Generic;
using System.Text;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Helpers
{
    public class ReservationsHelper
    {
        public static string GetPaymentStatus(bool isPaid)
        {
            return isPaid ? PickerConstans.ReservationIsPaid : PickerConstans.ReservationIsNotPaid;
        }

        public static string GetPlace(ReservationPlace place)
        {
            return place switch
            {
                ReservationPlace.AtTutor => PickerConstans.AtTutor,
                ReservationPlace.AtStudent => PickerConstans.AtStudent,
                ReservationPlace.Online => PickerConstans.Online,
                _ => PickerConstans.LessonOtherPlace
            };
        }

        public static string GetRecurringReservationType(ReservationFrequency frequency)
        {
            return frequency switch
            {
                ReservationFrequency.Weekly => PickerConstans.WeeklyReservation,
                ReservationFrequency.OnceTwoWeeks => PickerConstans.OnceTwoWeeksReservation,
                ReservationFrequency.Monthly => PickerConstans.MonthlyReservation,
                _ => PickerConstans.OtherReservationType
            };
        }
    }
}