using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AdditionalOrderDtos;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.OrderViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        private int currentPage;
        private bool hasNext;
        private int itemTreshold = 5;
        private bool isRefreshing;

        private bool isPaid = true;
        private bool isNotPaid = true;
        private bool isInProgress = true;
        private bool isPending = true;
        private bool isRealized = true;
        private DateTime? receiptStartDate = DateTime.Now.AddMonths(-1);
        private DateTime? receiptEndDate = DateTime.Now;
        private DateTime? deadlineStart = DateTime.Now.AddDays(-14);
        private DateTime? deadlineEnd = DateTime.Now.AddMonths(1);

        public ObservableCollection<OrderDto> Orders { get; }

        public int ItemTreshold { get => itemTreshold; set => SetValue(ref itemTreshold, value); }
        public int CurrentPage { get => currentPage; set => SetValue(ref currentPage, value); }
        public bool HasNext { get => hasNext; set => SetValue(ref hasNext, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetValue(ref isRefreshing, value); }

        public bool IsPaid { get => isPaid; set => SetValue(ref isPaid, value); }
        public bool IsNotPaid { get => isNotPaid; set => SetValue(ref isNotPaid, value); }
        public bool IsInProgress { get => isInProgress; set => isInProgress = value; }
        public bool IsPending { get => isPending; set => isPending = value; }
        public bool IsRealized { get => isRealized; set => isRealized = value; }
        public DateTime? ReceiptStartDate { get => receiptStartDate; set => SetValue(ref receiptStartDate, value); }
        public DateTime? ReceiptEndDate { get => receiptEndDate; set => SetValue(ref receiptEndDate, value); }
        public DateTime? DeadlineStart { get => deadlineStart; set => SetValue(ref deadlineStart, value); }
        public DateTime? DeadlineEnd { get => deadlineEnd; set => SetValue(ref deadlineEnd, value); }


        public Command LoadOrdersCommand { get; }
        public Command ItemTresholdReachedCommand { get; }
        public Command NewOrderFormCommand { get; }
        public Command OpenFilteringPopupCommand { get; }
        public Command<OrderDto> OrderTappedCommand { get; }
        public Command PageAppearingCommand { get; }
        public Command<OrderDto> ChangeOrderStatusCommand { get; }
        public Command<OrderDto> ChangePaymentStatusCommand { get; }
        public Command FilterOrdersCommand { get; }

        private readonly IAdditionalOrderService orderService;

        public OrdersViewModel()
        {
            orderService = DependencyService.Get<IAdditionalOrderService>();
            Orders = new ObservableCollection<OrderDto>();
            LoadOrdersCommand = new Command(async () => await LoadOrders());
            ItemTresholdReachedCommand = new Command(async () => await OrdersTresholdReached());
            NewOrderFormCommand = new Command(async () => await OnNewOrderClick());
            OpenFilteringPopupCommand = new Command(async () => await OnFilteringOrdersClick());
            OrderTappedCommand = new Command<OrderDto>(async (order) => await OnOrderSelected(order));
            PageAppearingCommand = new Command(async () => await OnAppearing());
            ChangeOrderStatusCommand = new Command<OrderDto>(async (order) => await OnChangeOrderStatus(order));
            ChangePaymentStatusCommand = new Command<OrderDto>(async (order) => await OnChangePaymentStatus(order));
        }

        private async Task LoadOrders()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsRefreshing = true;

            Orders.Clear();
            var ordersCollection = await orderService.GetAdditionalOrdersAsync(new AdditionalOrderParameters { PageSize = 20 });
            CurrentPage = ordersCollection.Pagination.CurrentPage;
            HasNext = ordersCollection.Pagination.HasNext;
            foreach (var order in ordersCollection.Orders)
                Orders.Add(order);

            IsRefreshing = false;
            IsBusy = false;
        }

        private async Task OrdersTresholdReached()
        {
            if (IsBusy || !HasNext)
                return;

            IsBusy = true;
            var ordersCollection = await orderService.GetAdditionalOrdersAsync(new AdditionalOrderParameters { PageNumber = ++CurrentPage, PageSize = 10 });
            HasNext = ordersCollection.Pagination.HasNext;
            foreach (var order in ordersCollection.Orders)
                Orders.Add(order);

            IsBusy = false;
        }

        private async Task OnAppearing()
        {
            await LoadOrders();
        }

        private async Task OnOrderSelected(OrderDto order)
        {
            if (order == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(OrderDetailsTutorPage)}?{nameof(OrderDetailsViewModel.Id)}={order.Id}");
        }

        private async Task OnNewOrderClick()
        {
            await Shell.Current.GoToAsync($"{nameof(NewOrderTutotPage)}");
        }

        private async Task OnFilteringOrdersClick()
        {
            await PopupNavigation.Instance.PushAsync(new OrderFilteringTutorPopupPage());
        }

        private async Task OnChangeOrderStatus(OrderDto order)
        {
            if (order is null)
                return;

            var result = await Shell.Current.DisplayActionSheet("Zmiana statusu zlecenia", "Anuluj", null, "Oczekujące", "W realizacji", "Zrealizowane");
            var status = GetOrderStatus(result);
            if (await orderService.ChangeOrderStatusAsync(order.Id, status))
                DependencyService.Get<IToast>()?.MakeLongToast("Zmieniono status zlecenia");
            else
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj później!");
        }

        private async Task OnChangePaymentStatus(OrderDto order)
        {
            if (order is null)
                return;

            var result = await Shell.Current.DisplayActionSheet("Zmiana statusu płatności", "Anuluj", null, "Opłacone", "Nie opłacone");
            var status = GetPaymentStatus(result);
            if (await orderService.ChangePaymentStatusAsync(order.Id, status))
                DependencyService.Get<IToast>()?.MakeLongToast("Zmieniono status płatności");
            else
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj później!");
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