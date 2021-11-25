using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AdditionalOrderDtos;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Helpers;
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
        private DateTime receiptStartDate = DateTime.Now.AddMonths(-1);
        private DateTime receiptEndDate = DateTime.Now;
        private DateTime deadlineStart = DateTime.Now.AddDays(-14);
        private DateTime deadlineEnd = DateTime.Now.AddMonths(1);
        private string sortBy;
        private bool isSortingByNameAsc;
        private bool isSortingByNameDesc;
        private bool isSortingByPriceAsc;
        private bool isSortingByPriceDesc;
        private bool isSortingByDeadlineAsc;
        private bool isSortingByDeadlineDesc;
        private bool isSortingByCreatedDateAsc;
        private bool isSortingByCreatedDateDesc = true;

        public ObservableCollection<OrderDto> Orders { get; }

        public int ItemTreshold { get => itemTreshold; set => SetValue(ref itemTreshold, value); }
        public int CurrentPage { get => currentPage; set => SetValue(ref currentPage, value); }
        public bool HasNext { get => hasNext; set => SetValue(ref hasNext, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetValue(ref isRefreshing, value); }

        public bool IsPaid
        {
            get => isPaid;
            set
            {
                if (!value && !IsNotPaid)
                    IsNotPaid = true;
                SetValue(ref isPaid, value);
            }
        }
        public bool IsNotPaid
        {
            get => isNotPaid;
            set
            {
                if (!value && !IsPaid)
                    IsPaid = true;
                SetValue(ref isNotPaid, value);
            }
        }
        public bool IsInProgress
        {
            get => isInProgress;
            set
            {
                if (!value && !IsPending && !IsRealized)
                {
                    DependencyService.Get<IToast>()?.MakeShortToast("Musi być wybrany minimum 1 status!");
                    IsPending = true;
                }
                SetValue(ref isInProgress, value);
            }
        }
        public bool IsPending
        {
            get => isPending;
            set
            {
                if (!value && !IsInProgress && !IsRealized)
                {
                    DependencyService.Get<IToast>()?.MakeShortToast("Musi być wybrany minimum 1 status!");
                    IsInProgress = true;
                }
                SetValue(ref isPending, value);
            }
        }
        public bool IsRealized
        {
            get => isRealized;
            set
            {
                if (!value && !IsInProgress && !IsPending)
                {
                    DependencyService.Get<IToast>()?.MakeShortToast("Musi być wybrany minimum 1 status!");
                    IsInProgress = true;
                }
                SetValue(ref isRealized, value);
            }
        }
        public DateTime ReceiptStartDate { get => receiptStartDate; set => SetValue(ref receiptStartDate, value); }
        public DateTime ReceiptEndDate { get => receiptEndDate; set => SetValue(ref receiptEndDate, value); }
        public DateTime DeadlineStart { get => deadlineStart; set => SetValue(ref deadlineStart, value); }
        public DateTime DeadlineEnd { get => deadlineEnd; set => SetValue(ref deadlineEnd, value); }

        public string SortBy { get => sortBy; set => SetValue(ref sortBy, value); }

        public bool IsSortingByNameAsc
        {
            get => isSortingByNameAsc;
            set
            {
                OnSortByNameAsc();
                SetValue(ref isSortingByNameAsc, value);
            }
        }
        public bool IsSortingByNameDesc
        {
            get => isSortingByNameDesc;
            set
            {
                OnSortByNameDesc();
                SetValue(ref isSortingByNameDesc, value);
            }
        }
        public bool IsSortingByPriceAsc
        {
            get => isSortingByPriceAsc;
            set
            {
                OnSortByPriceAsc();
                SetValue(ref isSortingByPriceAsc, value);
            }
        }
        public bool IsSortingByPriceDesc
        {
            get => isSortingByPriceDesc;
            set
            {
                OnSortByPriceDesc();
                SetValue(ref isSortingByPriceDesc, value);
            }
        }
        public bool IsSortingByDeadlineAsc
        {
            get => isSortingByDeadlineAsc;
            set
            {
                OnSortByDeadlineAsc();
                SetValue(ref isSortingByDeadlineAsc, value);
            }
        }
        public bool IsSortingByDeadlineDesc
        {
            get => isSortingByDeadlineDesc;
            set
            {
                OnSortByDeadlineDesc();
                SetValue(ref isSortingByDeadlineDesc, value);
            }
        }
        public bool IsSortingByCreatedDateAsc
        {
            get => isSortingByCreatedDateAsc;
            set
            {
                OnSortByCreatedDateAsc();
                SetValue(ref isSortingByCreatedDateAsc, value);
            }
        }
        public bool IsSortingByCreatedDateDesc
        {
            get => isSortingByCreatedDateDesc;
            set
            {
                OnSortByCreatedDateDesc();
                SetValue(ref isSortingByCreatedDateDesc, value);
            }
        }


        public Command LoadOrdersCommand { get; }
        public Command ItemTresholdReachedCommand { get; }
        public Command NewOrderFormCommand { get; }
        public Command OpenFilteringPopupCommand { get; }
        public Command OpenSortingPopupCommand { get; }
        public Command<OrderDto> OrderTappedCommand { get; }
        public Command PageAppearingCommand { get; }
        public Command<OrderDto> ChangeOrderStatusCommand { get; }
        public Command<OrderDto> ChangePaymentStatusCommand { get; }
        public Command FilterOrdersCommand { get; }
        public Command SortingOrdersCommand { get; }

        public Command SortByNameAscCommand { get; }
        public Command SortByNameDescCommand { get; }
        public Command SortByPriceAscCommand { get; }
        public Command SortByPriceDescCommand { get; }
        public Command SortByDeadlineAscCommand { get; }
        public Command SortByDeadlineDescCommand { get; }
        public Command SortByCreatedDateAscCommand { get; }
        public Command SortByCreatedDateDescCommand { get; }

        private readonly IAdditionalOrderService orderService;

        public OrdersViewModel()
        {
            orderService = DependencyService.Get<IAdditionalOrderService>();
            Orders = new ObservableCollection<OrderDto>();

            LoadOrdersCommand = new Command(async () => await LoadOrders());
            ItemTresholdReachedCommand = new Command(async () => await OrdersTresholdReached());
            NewOrderFormCommand = new Command(async () => await OnNewOrderClick());
            OpenFilteringPopupCommand = new Command(async () => await OnOpenFilteringPopup());
            OpenSortingPopupCommand = new Command(async () => await OnOpenSortingPopup());
            OrderTappedCommand = new Command<OrderDto>(async (order) => await OnOrderSelected(order));
            PageAppearingCommand = new Command(async () => await OnAppearing());
            ChangeOrderStatusCommand = new Command<OrderDto>(async (order) => await OnChangeOrderStatus(order));
            ChangePaymentStatusCommand = new Command<OrderDto>(async (order) => await OnChangePaymentStatus(order));
            FilterOrdersCommand = new Command(async () => await OnFilterOrders());
            SortingOrdersCommand = new Command(async () => await OnSortingOrders());

            SortByNameAscCommand = new Command(OnSortByNameAsc);
            SortByNameDescCommand = new Command(OnSortByNameDesc);
            SortByPriceAscCommand = new Command(OnSortByPriceAsc);
            SortByPriceDescCommand = new Command(OnSortByPriceDesc);
            SortByDeadlineAscCommand = new Command(OnSortByDeadlineAsc);
            SortByDeadlineDescCommand = new Command(OnSortByDeadlineDesc);
            SortByCreatedDateAscCommand = new Command(OnSortByCreatedDateAsc);
            SortByCreatedDateDescCommand = new Command(OnSortByCreatedDateDesc);

            SortBy = $"{nameof(OrderDetailsDto.ReceiptDate)} desc";
        }

        private async Task LoadOrders()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsRefreshing = true;

            Orders.Clear();
            var ordersCollection = await orderService.GetAdditionalOrdersAsync(new
                AdditionalOrderParameters(IsPaid, IsNotPaid, IsInProgress, IsPending, isRealized, ReceiptStartDate, ReceiptEndDate, DeadlineStart, DeadlineEnd, 20, 1, SortBy));
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
            var ordersCollection = await orderService.GetAdditionalOrdersAsync(new
                AdditionalOrderParameters(IsPaid, IsNotPaid, IsInProgress, IsPending, isRealized, ReceiptStartDate, ReceiptEndDate, DeadlineStart, DeadlineEnd, 20, ++CurrentPage, SortBy));
            HasNext = ordersCollection.Pagination.HasNext;
            foreach (var order in ordersCollection.Orders)
                Orders.Add(order);

            IsBusy = false;
        }

        private async Task OnFilterOrders()
        {
            await LoadOrders();
            await PopupNavigation.Instance.PopAsync();
        }

        private async Task OnSortingOrders()
        {
            SetSortingParams();
            await LoadOrders();
            await PopupNavigation.Instance.PopAsync();
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

        private async Task OnOpenFilteringPopup()
        {
            await PopupNavigation.Instance.PushAsync(new OrderFilteringTutorPopupPage(this));
        }

        private async Task OnOpenSortingPopup()
        {
            await PopupNavigation.Instance.PushAsync(new OrderSortingTutorPopupPage(this));
        }

        private async Task OnChangeOrderStatus(OrderDto order)
        {
            if (order is null)
                return;

            var result = await Shell.Current.DisplayActionSheet("Zmiana statusu zlecenia", "Anuluj", null, "Oczekujące", "W realizacji", "Zrealizowane");
            var status = GetOrderStatus(result);
            if (await orderService.ChangeOrderStatusAsync(order.Id, status))
            {
                await LoadOrders();
                DependencyService.Get<IToast>()?.MakeLongToast("Zmieniono status zlecenia");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj później!");
            }
        }

        private async Task OnChangePaymentStatus(OrderDto order)
        {
            if (order is null)
                return;

            var result = await Shell.Current.DisplayActionSheet("Zmiana statusu płatności", "Anuluj", null, "Opłacone", "Nie opłacone");
            var status = GetPaymentStatus(result);
            if (await orderService.ChangePaymentStatusAsync(order.Id, status))
            {
                await LoadOrders();
                DependencyService.Get<IToast>()?.MakeLongToast("Zmieniono status płatności");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj później!");
            }
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

        private void SetSortingParams()
        {
            if (IsSortingByNameAsc)
                SortBy = $"{nameof(OrderDetailsDto.Name)}";
            else if (IsSortingByNameDesc)
                SortBy = $"{nameof(OrderDetailsDto.Name)} desc";
            else if (IsSortingByPriceAsc)
                SortBy = $"{nameof(OrderDetailsDto.Cost)}";
            else if (IsSortingByPriceDesc)
                SortBy = $"{nameof(OrderDetailsDto.Cost)} desc";
            else if (IsSortingByDeadlineAsc)
                SortBy = $"{nameof(OrderDetailsDto.Deadline)}";
            else if (IsSortingByDeadlineDesc)
                SortBy = $"{nameof(OrderDetailsDto.Deadline)} desc";
            else if (IsSortingByCreatedDateAsc)
                SortBy = $"{nameof(OrderDetailsDto.ReceiptDate)}";
            else if (IsSortingByCreatedDateDesc)
                SortBy = $"{nameof(OrderDetailsDto.ReceiptDate)} desc";
        }

        private async void OnSortByCreatedDateDesc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, false, false, false, false, false, false, true));
        }

        private async void OnSortByCreatedDateAsc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, false, false, false, false, false, true, false));
        }

        private async void OnSortByDeadlineDesc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, false, false, false, false, true, false, false));
        }

        private async void OnSortByDeadlineAsc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, false, false, false, true, false, false, false));
        }

        private async void OnSortByPriceDesc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, false, false, true, false, false, false, false));
        }

        private async void OnSortByPriceAsc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, false, true, false, false, false, false, false));
        }

        private async void OnSortByNameDesc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, true, false, false, false, false, false, false));
        }

        private async void OnSortByNameAsc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(true, false, false, false, false, false, false, false));
        }

        private async Task SortOrdersAsync(OrderSortingRadioButtonsActivity buttonsActivity)
        {
            if (IsBusy || PopupNavigation.Instance.PopupStack.Count == 0)
                return;

            IsBusy = true;
            IsSortingByNameAsc = buttonsActivity.IsSortingByNameAsc;
            IsSortingByNameDesc = buttonsActivity.IsSortingByNameDesc;
            IsSortingByPriceAsc = buttonsActivity.IsSortingByPriceAsc;
            IsSortingByPriceDesc = buttonsActivity.IsSortingByPriceDesc;
            IsSortingByDeadlineAsc = buttonsActivity.IsSortingByDeadlineAsc;
            IsSortingByDeadlineDesc = buttonsActivity.IsSortingByDeadlineDesc;
            IsSortingByCreatedDateAsc = buttonsActivity.IsSortingByCreatedDateAsc;
            IsSortingByCreatedDateDesc = buttonsActivity.IsSortingByCreatedDateDesc;
            IsBusy = false;

            SetSortingParams();
            await LoadOrders();

            await PopupNavigation.Instance.PopAsync();
        }
    }
}