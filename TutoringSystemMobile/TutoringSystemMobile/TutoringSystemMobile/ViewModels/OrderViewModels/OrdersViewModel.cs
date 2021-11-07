using System.Collections.ObjectModel;
using System.Windows.Input;
using TutoringSystemMobile.Commands.OrderCommands;
using TutoringSystemMobile.Models.AdditionalOrderDtos;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.OrderViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        private OrderDto selectedOrder;
        public ObservableCollection<OrderDto> Orders { get; }

        public OrderDto SelectedOrder
        {
            get => selectedOrder;
            set
            {
                SetValue(ref selectedOrder, value);
                OnOrderSelected(value);
            }
        }

        public ICommand LoadOrdersCommand { get; }
        public Command NewOrderFormCommand { get; }
        public Command FilterOrdersCommand { get; }
        public Command<OrderDto> OrderTapped { get; }
        public Command PageAppearingCommand { get; }
        public Command<OrderDto> ChangeOrderStatusCommand { get; }
        public Command<OrderDto> ChangePaymentStatusCommand { get; }

        public readonly IAdditionalOrderService orderService;

        public OrdersViewModel()
        {
            Orders = new ObservableCollection<OrderDto>();
            orderService = DependencyService.Get<IAdditionalOrderService>();
            LoadOrdersCommand = new LoadOrdersCommand(this, orderService);
            NewOrderFormCommand = new Command(OnNewOrderClick);
            FilterOrdersCommand = new Command(OnFilteringOrdersClick);
            OrderTapped = new Command<OrderDto>(OnOrderSelected);
            PageAppearingCommand = new Command(OnAppearing);
            ChangeOrderStatusCommand = new Command<OrderDto>(OnChangeOrderStatus);
            ChangePaymentStatusCommand = new Command<OrderDto>(OnChangePaymentStatus);
        }

        private void OnAppearing()
        {
            IsBusy = true;
            selectedOrder = null;
        }

        private async void OnOrderSelected(OrderDto order)
        {
            if (order == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(OrderDetailsTutorPage)}?{nameof(OrderDetailsViewModel.Id)}={order.Id}");
        }

        private async void OnNewOrderClick()
        {
            await Shell.Current.GoToAsync($"{nameof(NewOrderTutotPage)}");
        }

        private async void OnFilteringOrdersClick()
        {
            await Shell.Current.GoToAsync($"{nameof(OrderFilteringTutorPage)}");
        }

        private async void OnChangeOrderStatus(OrderDto order)
        {
            var result = await Shell.Current.DisplayActionSheet("Zmiana statusu zlecenia", "Anuluj", null, "Oczekujące", "W realizacji", "Zrealizowane");
            var status = GetOrderStatus(result);
            if (await orderService.ChangeOrderStatusAsync(order.Id, status))
                DependencyService.Get<IToast>()?.MakeToast("Zmieniono status zlecenia");
            else
                DependencyService.Get<IToast>()?.MakeToast("Błąd! Spróbuj później!");
        }

        private async void OnChangePaymentStatus(OrderDto order)
        {
            var result = await Shell.Current.DisplayActionSheet("Zmiana statusu płatności", "Anuluj", null, "Opłacone", "Nie opłacone");
            var status = GetPaymentStatus(result);
            if (await orderService.ChangePaymentStatusAsync(order.Id, status))
                DependencyService.Get<IToast>()?.MakeToast("Zmieniono status płatności");
            else
                DependencyService.Get<IToast>()?.MakeToast("Błąd! Spróbuj później!");
        }

        private AdditionalOrderStatus GetOrderStatus(string statusStringPl)
        {
            switch (statusStringPl)
            {
                case "Oczekujące":
                    return AdditionalOrderStatus.Pending;
                case "W realizacji":
                    return AdditionalOrderStatus.InProgress;
                case "Zrealizowane":
                    return AdditionalOrderStatus.Realized;
                default:
                    return AdditionalOrderStatus.Pending;
            }
        }

        private bool GetPaymentStatus(string paymentStatusStringPl)
        {
            switch (paymentStatusStringPl)
            {
                case "Opłacone":
                    return true;
                case "Nie opłacone":
                    return false;
                default:
                    return false;
            }
        }
    }
}