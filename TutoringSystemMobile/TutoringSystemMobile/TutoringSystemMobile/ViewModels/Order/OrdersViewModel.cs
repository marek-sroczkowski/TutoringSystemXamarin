﻿using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.AdditionalOrder;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Order
{
    public class OrdersViewModel : BaseViewModel
    {
        private int currentPage;
        private bool hasNext;
        private bool isRefreshing;
        private string sortBy;

        public ObservableCollection<OrderDto> Orders { get; }

        public int CurrentPage { get => currentPage; set => SetValue(ref currentPage, value); }
        public bool HasNext { get => hasNext; set => SetValue(ref hasNext, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetValue(ref isRefreshing, value); }

        public string SortBy { get => sortBy; set => SetValue(ref sortBy, value); }

        public AdditionalOrderParameters OrderParameters { get; set; }

        public Command LoadOrdersCommand { get; }
        public Command ItemTresholdReachedCommand { get; }
        public Command NewOrderFormCommand { get; }
        public Command OpenFilteringPopupCommand { get; }
        public Command OpenSortingPopupCommand { get; }
        public Command<OrderDto> OrderTappedCommand { get; }
        public Command PageAppearingCommand { get; }
        public Command<OrderDto> ChangeOrderStatusCommand { get; }
        public Command<OrderDto> ChangePaymentStatusCommand { get; }

        private readonly IAdditionalOrderService orderService = DependencyService.Get<IAdditionalOrderService>();

        public OrdersViewModel()
        {
            SubscribeEvents();
            OrderParameters = new AdditionalOrderParameters();
            Orders = new ObservableCollection<OrderDto>();
            SortBy = SortingConstans.SortByReceiptDateDesc;

            LoadOrdersCommand = new Command(async () => await OnLoadOrders());
            ItemTresholdReachedCommand = new Command(async () => await OrdersTresholdReached());
            NewOrderFormCommand = new Command(async () => await OnNewOrderClick());
            OpenFilteringPopupCommand = new Command(async () => await OnOpenFilteringPopup());
            OpenSortingPopupCommand = new Command(async () => await OnOpenSortingPopup());
            OrderTappedCommand = new Command<OrderDto>(async (order) => await OnOrderSelected(order));
            PageAppearingCommand = new Command(async () => await OnAppearing());
            ChangeOrderStatusCommand = new Command<OrderDto>(async (order) => await OnChangeOrderStatus(order));
            ChangePaymentStatusCommand = new Command<OrderDto>(async (order) => await OnChangePaymentStatus(order));
        }

        private void SubscribeEvents()
        {
            MessagingCenter.Subscribe<OrderSortingViewModel>(this, MessagingCenterConstans.OrderSorting, async (sender) =>
            {
                SortBy = sender.SortBy;
                await OnLoadOrders();
            });

            MessagingCenter.Subscribe<OrderFilteringViewModel>(this, MessagingCenterConstans.OrderFiltering, async (sender) =>
            {
                OrderParameters.DeadlineEnd = sender.DeadlineEnd;
                OrderParameters.DeadlineStart = sender.DeadlineStart;
                OrderParameters.IsInProgress = sender.IsInProgress;
                OrderParameters.IsNotPaid = sender.IsNotPaid;
                OrderParameters.IsPaid = sender.IsPaid;
                OrderParameters.IsPending = sender.IsPending;
                OrderParameters.IsRealized = sender.IsRealized;
                OrderParameters.ReceiptEndDate = sender.ReceiptEndDate;
                OrderParameters.ReceiptStartDate = sender.ReceiptStartDate;
                await OnLoadOrders();
            });
        }

        private async Task OnLoadOrders()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            IsRefreshing = true;
            await GetOrdersAsync();
            IsRefreshing = false;
            IsBusy = false;
        }

        private async Task GetOrdersAsync()
        {
            OrderParameters.PageNumber = 1;
            OrderParameters.PageSize = 20;
            OrderParameters.OrderBy = SortBy;

            var ordersCollection = await orderService.GetOrdersAsync(OrderParameters);
            CurrentPage = ordersCollection.Pagination.CurrentPage;
            HasNext = ordersCollection.Pagination.HasNext;
            Orders.Clear();
            ordersCollection.Orders.ToList().ForEach(order => Orders.Add(order));
        }

        private async Task OrdersTresholdReached()
        {
            if (IsBusy || !HasNext)
            {
                return;
            }

            IsBusy = true;
            OrderParameters.PageNumber = ++CurrentPage;
            OrderParameters.PageSize = 20;
            OrderParameters.OrderBy = SortBy;

            var ordersCollection = await orderService.GetOrdersAsync(OrderParameters);
            HasNext = ordersCollection.Pagination.HasNext;
            ordersCollection.Orders.ToList().ForEach(order => Orders.Add(order));

            IsBusy = false;
        }

        private async Task OnAppearing()
        {
            await OnLoadOrders();
        }

        private async Task OnOrderSelected(OrderDto order)
        {
            if (order == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(OrderDetailsTutorPage)}?{nameof(OrderDetailsViewModel.Id)}={order.Id}");
        }

        private async Task OnNewOrderClick()
        {
            await Shell.Current.GoToAsync($"{nameof(NewOrderTutotPage)}");
        }

        private async Task OnOpenFilteringPopup()
        {
            await PopupNavigation.Instance.PushAsync(new OrderFilteringTutorPopupPage(OrderParameters));
        }

        private async Task OnOpenSortingPopup()
        {
            await PopupNavigation.Instance.PushAsync(new OrderSortingTutorPopupPage(SortBy));
        }

        private async Task OnChangeOrderStatus(OrderDto order)
        {
            if (order is null)
            {
                return;
            }

            var result = await Shell.Current.DisplayActionSheet(AlertConstans.ChangeOrderStatus, GeneralConstans.Cancel, null, PickerConstans.PendingOrder, PickerConstans.InProgressOrder, PickerConstans.RealizedOrder);
            if (result is null || result == GeneralConstans.Cancel)
            {
                return;
            }

            var status = OrdersHelper.GetStatus(result);
            if (await orderService.ChangeOrderStatusAsync(order.Id, status))
            {
                await OnLoadOrders();
                ToastHelper.MakeLongToast(ToastConstans.ChangedOrderStatus);
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async Task OnChangePaymentStatus(OrderDto order)
        {
            if (order is null)
            {
                return;
            }

            var result = await Shell.Current.DisplayActionSheet(AlertConstans.ChangeOrderPaymentStatus, GeneralConstans.Cancel, null, PickerConstans.OrderIsPaid, PickerConstans.OrderIsNotPaid);
            if (result is null || result == GeneralConstans.Cancel)
            {
                return;
            }

            var status = OrdersHelper.GetPaymentStatus(result);
            if (await orderService.ChangePaymentStatusAsync(order.Id, status))
            {
                await OnLoadOrders();
                ToastHelper.MakeLongToast(ToastConstans.ChangedOrderPaymentStatus);
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }
    }
}