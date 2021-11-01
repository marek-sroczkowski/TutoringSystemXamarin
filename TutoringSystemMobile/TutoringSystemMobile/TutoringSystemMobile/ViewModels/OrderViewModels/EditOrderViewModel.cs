using System;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.OrderViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EditOrderViewModel : BaseViewModel
    {
        private long id;
        private string name;
        private DateTime deadline;
        private double cost;
        private bool isPaid;
        private AdditionalOrderStatus status;
        private DateTime receiptDate;
        private string description;

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
        public DateTime ReceiptDate { get => receiptDate; set => SetValue(ref receiptDate, value); }
        public string Description { get => description; set => SetValue(ref description, value); }

        private readonly IAdditionalOrderService orderService;

        public EditOrderViewModel()
        {
            orderService = DependencyService.Get<IAdditionalOrderService>();
        }

        public async void LoadOrderById(long orderId)
        {
            var order = await orderService.GetAdditionalOrderByIdAsync(orderId);

            Name = order.Name;
            Deadline = order.Deadline;
            Cost = order.Cost;
            IsPaid = order.IsPaid;
            Status = order.Status;
            ReceiptDate = order.ReceiptDate;
            Description = order.Description;
        }
    }
}
