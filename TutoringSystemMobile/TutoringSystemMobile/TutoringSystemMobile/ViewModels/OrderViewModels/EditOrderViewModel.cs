using System;
using System.Collections.Generic;
using System.Windows.Input;
using TutoringSystemMobile.Commands.OrderCommands;
using TutoringSystemMobile.Constans;
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
        private DateTime? deadline;
        private string cost;
        private bool isPaid;
        private AdditionalOrderStatus status;
        private string description;
        private string selectedStatus;

        public long Id
        {
            get => id;
            set
            {
                id = value;
                LoadOrderById(id);
            }
        }

        public string SelectedStatus
        {
            get =>  selectedStatus;
            set
            {
                SetValue(ref selectedStatus, value);
                switch (selectedStatus)
                {
                    case PickerConstans.PendingOrder:
                        Status = AdditionalOrderStatus.Pending;
                        break;
                    case PickerConstans.InProgressOrder:
                        Status = AdditionalOrderStatus.InProgress;
                        break;
                    case PickerConstans.RealizedOrder:
                        Status = AdditionalOrderStatus.Realized;
                        break;
                }
            }
        }

        public string Name { get => name; set => SetValue(ref name, value); }
        public DateTime? Deadline { get => deadline; set => SetValue(ref deadline, value); }
        public string Cost { get => cost; set => SetValue(ref cost, value); }
        public bool IsPaid { get => isPaid; set => SetValue(ref isPaid, value); }
        public AdditionalOrderStatus Status { get => status; set => SetValue(ref status, value); }
        public string Description { get => description; set => SetValue(ref description, value); }

        public List<string> Statuses { get; set; }

        public ICommand EditOrderCommand { get; }

        private readonly IAdditionalOrderService orderService;

        public EditOrderViewModel()
        {
            SetOrderStatuses();
            orderService = DependencyService.Get<IAdditionalOrderService>();
            EditOrderCommand = new EditOrderCommand(this, orderService);
        }

        private void SetOrderStatuses()
        {
            Statuses = new List<string>
            {
                PickerConstans.PendingOrder,
                PickerConstans.InProgressOrder,
                PickerConstans.RealizedOrder
            };
        }

        private async void LoadOrderById(long orderId)
        {
            var order = await orderService.GetAdditionalOrderByIdAsync(orderId);

            Name = order.Name;
            Deadline = order.Deadline;
            Cost = order.Cost.ToString();
            IsPaid = order.IsPaid;
            Status = order.Status;
            Description = order.Description;
            SetSelectedStatus();
        }

        public void SetSelectedStatus()
        {
            switch (Status)
            {
                case AdditionalOrderStatus.Pending:
                    SelectedStatus = PickerConstans.PendingOrder;
                    break;
                case AdditionalOrderStatus.InProgress:
                    SelectedStatus = PickerConstans.InProgressOrder;
                    break;
                case AdditionalOrderStatus.Realized:
                    SelectedStatus = PickerConstans.RealizedOrder;
                    break;
            }
        }
    }
}