using System;
using System.Collections.Generic;
using System.Text;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.OrderViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class OrderDetailsViewModel : BaseViewModel
    {
        private long id;
        private string name;
        private DateTime deadline;
        private double cost;
        private bool isPaid;
        private AdditionalOrderStatus status;

        public long Id
        {
            get => id;
            set
            {
                id = value;
                LoadOrderById(id);
            }
        }

        public string Name { get => name; set => SetValue(ref name, value); }
        public DateTime Deadline { get => deadline; set => SetValue(ref deadline, value); }
        public double Cost { get => cost; set => SetValue(ref cost, value); }
        public bool IsPaid { get => isPaid; set => SetValue(ref isPaid, value); }
        public AdditionalOrderStatus Status { get => status; set => SetValue(ref status, value); }

        public OrderDetailsViewModel()
        {
        }

        public async void LoadOrderById(long orderId)
        {
            AdditionalOrderService orderService = new AdditionalOrderService();
            var order = await orderService.GetAdditionalOrderByIdAsync(orderId);
            if (order is null)
                return;

            Name = order.Name;
            Deadline = order.Deadline;
            Cost = order.Cost;
            IsPaid = order.IsPaid;
            Status = order.Status;
        }
    }
}