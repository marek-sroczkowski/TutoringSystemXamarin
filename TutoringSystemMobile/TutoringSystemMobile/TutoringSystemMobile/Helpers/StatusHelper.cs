using System.Collections.Generic;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Helpers
{
    public class StatusHelper
    {
        public static List<string> GetOrderStatusesCollection()
        {
            return new List<string>
            {
                PickerConstans.PendingOrder,
                PickerConstans.InProgressOrder,
                PickerConstans.RealizedOrder
            };
        }

        public static AdditionalOrderStatus GetOrderStatus(string status)
        {
            return status switch
            {
                PickerConstans.PendingOrder => AdditionalOrderStatus.Pending,
                PickerConstans.InProgressOrder => AdditionalOrderStatus.InProgress,
                PickerConstans.RealizedOrder => AdditionalOrderStatus.Realized,
                _ => AdditionalOrderStatus.Pending
            };
        }

        public static string GetOrderStatus(AdditionalOrderStatus status)
        {
            return status switch
            {
                AdditionalOrderStatus.Pending => PickerConstans.PendingOrder,
                AdditionalOrderStatus.InProgress => PickerConstans.InProgressOrder,
                AdditionalOrderStatus.Realized => PickerConstans.RealizedOrder,
                _ => PickerConstans.PendingOrder,
            };
        }

        public static string GetOrderPaymentStatus(bool isPaid)
        {
            return isPaid ? PickerConstans.OrderIsPaid : PickerConstans.OrderIsNotPaid;
        }

        public static bool GetOrderPaymentStatus(string paymentStatus)
        {
            return paymentStatus switch
            {
                PickerConstans.OrderIsPaid => true,
                _ => false,
            };
        }
    }
}
