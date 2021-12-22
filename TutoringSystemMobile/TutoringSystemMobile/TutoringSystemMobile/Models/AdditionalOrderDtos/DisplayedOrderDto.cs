using System;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;
using Xamarin.Forms;

namespace TutoringSystemMobile.Models.AdditionalOrderDtos
{
    public class DisplayedOrderDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Color DeadlineTextColor { get; set; }
        public string Deadline { get; set; }
        public string Cost { get; set; }
        public string OrderStatus { get; set; }

        public DisplayedOrderDto(OrderDto order)
        {
            Id = order.Id;
            Name = order.Name;
            Deadline = order.Deadline.ToShortDateString();
            Cost = $"{order.Cost} {GeneralConstans.Pln}";
            SetDeadlineTextColor(order.Deadline);
            SetOrderStatus(order.Status);
        }

        private void SetDeadlineTextColor(DateTime deadline)
        {
            if (deadline.Date < DateTime.Now.Date)
                DeadlineTextColor = Color.Red;
            else if (deadline.Date >= DateTime.Now.Date && deadline.Date < DateTime.Now.AddDays(2))
                DeadlineTextColor = Color.DarkOrange;
            else
                DeadlineTextColor = Color.Green;
        }

        private void SetOrderStatus(AdditionalOrderStatus status)
        {
            OrderStatus = status switch
            {
                AdditionalOrderStatus.Pending => PickerConstans.PendingOrder,
                AdditionalOrderStatus.InProgress => PickerConstans.InProgressOrder,
                AdditionalOrderStatus.Realized => PickerConstans.RealizedOrder,
                _ => PickerConstans.PendingOrder
            };
        }
    }
}