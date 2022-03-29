using System.Collections.Generic;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Helpers
{
    public class OrdersHelper
    {
        public static List<string> GetStatusesCollection()
        {
            return new List<string>
            {
                PickerConstans.PendingOrder,
                PickerConstans.InProgressOrder,
                PickerConstans.RealizedOrder
            };
        }

        public static AdditionalOrderStatus GetStatus(string status)
        {
            return status switch
            {
                PickerConstans.PendingOrder => AdditionalOrderStatus.Pending,
                PickerConstans.InProgressOrder => AdditionalOrderStatus.InProgress,
                PickerConstans.RealizedOrder => AdditionalOrderStatus.Realized,
                _ => AdditionalOrderStatus.Pending
            };
        }

        public static string GetStatus(AdditionalOrderStatus status)
        {
            return status switch
            {
                AdditionalOrderStatus.Pending => PickerConstans.PendingOrder,
                AdditionalOrderStatus.InProgress => PickerConstans.InProgressOrder,
                AdditionalOrderStatus.Realized => PickerConstans.RealizedOrder,
                _ => PickerConstans.PendingOrder,
            };
        }

        public static string GetPaymentStatus(bool isPaid)
        {
            return isPaid ? PickerConstans.OrderIsPaid : PickerConstans.OrderIsNotPaid;
        }

        public static bool GetPaymentStatus(string paymentStatus)
        {
            return paymentStatus switch
            {
                PickerConstans.OrderIsPaid => true,
                _ => false,
            };
        }
    }
}
