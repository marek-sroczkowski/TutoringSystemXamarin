using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
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
        private DateTime receiptDate;
        private string description;
        private string paidStatus;
        private string orderStatus;

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
        public string PaidStatus { get => paidStatus; set => SetValue(ref paidStatus, value); }
        public string OrderStatus { get => orderStatus; set => SetValue(ref orderStatus, value); }

        public Command EditOrderCommand { get; }
        public Command RemoveOrderCommand { get; }

        private readonly IAdditionalOrderService orderService;

        public OrderDetailsViewModel()
        {
            orderService = DependencyService.Get<IAdditionalOrderService>();
            EditOrderCommand = new Command(OnRedirectToOrderEditPage);
            RemoveOrderCommand = new Command(OnRemoveRequest);
        }

        private async void LoadOrderById(long orderId)
        {
            IsBusy = true;
            var order = await orderService.GetAdditionalOrderByIdAsync(orderId);

            Name = order.Name;
            Deadline = order.Deadline;
            Cost = order.Cost;
            IsPaid = order.IsPaid;
            Status = order.Status;
            ReceiptDate = order.ReceiptDate;
            Description = order.Description;
            SetPaidStatus();
            SetOrderStatus();

            IsBusy = false;
        }

        private void SetPaidStatus()
        {
            if (isPaid)
                PaidStatus = "Zapłacono";
            else
                PaidStatus = "Nie zapłacono";
        }

        private void SetOrderStatus()
        {
            switch (Status)
            {
                case AdditionalOrderStatus.Pending:
                    OrderStatus = "Oczekujące";
                    break;
                case AdditionalOrderStatus.InProgress:
                    OrderStatus = "W realizacji";
                    break;
                case AdditionalOrderStatus.Realized:
                    OrderStatus = "Zrealizowane";
                    break;
            }
        }

        private async void OnRedirectToOrderEditPage()
        {
            await Shell.Current.GoToAsync($"{nameof(EditOrderTutorPage)}?{nameof(EditOrderViewModel.Id)}={Id}");
        }

        private async void OnRemoveRequest()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Uwaga!", "Czy na pewno chcesz usunąć to zlecenie?", "Tak", "Nie");
            if (result)
                await RemoveOrderAsync();
        }

        private async Task RemoveOrderAsync()
        {
            var removed = await orderService.DeleteAdditionalOrderAsync(Id);
            if (removed)
            {
                DependencyService.Get<IToast>()?.MakeToast("Usunięto zlecenie!");
                await Shell.Current.GoToAsync($"//{nameof(OrdersTutorPage)}");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeToast("Błąd! Spróbuj później!");
            }
        }
    }
}