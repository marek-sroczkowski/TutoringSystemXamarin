using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.AdditionalOrder;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Order
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
                Status = StatusHelper.GetOrderStatus(selectedStatus);
            }
        }

        public string Name { get => name; set => SetValue(ref name, value); }
        public DateTime? Deadline { get => deadline; set => SetValue(ref deadline, value); }
        public string Cost { get => cost; set => SetValue(ref cost, value); }
        public bool IsPaid { get => isPaid; set => SetValue(ref isPaid, value); }
        public AdditionalOrderStatus Status { get => status; set => SetValue(ref status, value); }
        public string Description { get => description; set => SetValue(ref description, value); }

        public List<string> Statuses { get; set; }

        public Command EditOrderCommand { get; }

        private readonly IAdditionalOrderService orderService;

        public EditOrderViewModel()
        {
            Statuses = StatusHelper.GetOrderStatusesCollection();
            orderService = DependencyService.Get<IAdditionalOrderService>();
            EditOrderCommand = new Command(async () => await OnEditOrder(), CanEditOrder);
            PropertyChanged += (_, __) => EditOrderCommand.ChangeCanExecute();
        }

        public bool CanEditOrder()
        {
            return !Name.IsEmpty()
                && !Cost.IsEmpty()
                && double.TryParse(Cost, out double cost)
                && cost > 0
                && Deadline.HasValue
                && !IsBusy;
        }

        private async Task OnEditOrder()
        {
            IsBusy = true;
            var updatedOrder = new UpdatedOrderDto(Id, Name, Deadline.Value, Description, Cost.ToDouble(), IsPaid, Status);
            bool updated = await orderService.UpdateAdditionalOrderAsync(updatedOrder);
            IsBusy = false;

            if (updated)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.Updated);
                await Shell.Current.GoToAsync($"//{nameof(OrdersTutorPage)}/{nameof(OrderDetailsTutorPage)}?{nameof(OrderDetailsViewModel.Id)}={Id}");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
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
            SelectedStatus = StatusHelper.GetOrderStatus(Status);
        }
    }
}