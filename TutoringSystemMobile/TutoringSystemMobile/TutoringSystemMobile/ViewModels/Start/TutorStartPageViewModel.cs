using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Dtos.AdditionalOrder;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Reservation;
using TutoringSystemMobile.Models.Dtos.StudentRequest;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Synchronization;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.Order;
using TutoringSystemMobile.Views;
using Xamarin.Forms;
using TutoringSystemMobile.ViewModels.Reservation;
using TutoringSystemMobile.Helpers;

namespace TutoringSystemMobile.ViewModels.Start
{
    public class TutorStartPageViewModel : BaseViewModel
    {
        private bool isRefreshing;
        private bool isNoReservations;
        private bool isNoOrders;
        private bool isReservations = true;
        private bool isOrders = true;
        private bool isRequests = false;

        public ObservableCollection<DisplayedReservationDto> Reservations { get; }
        public ObservableCollection<DisplayedOrderDto> Orders { get; }
        public ObservableCollection<StudentRequestDto> Requests { get; }

        public bool IsRefreshing { get => isRefreshing; set => SetValue(ref isRefreshing, value); }
        public bool IsNoReservations { get => isNoReservations; set => SetValue(ref isNoReservations, value); }
        public bool IsReservations { get => isReservations; set => SetValue(ref isReservations, value); }
        public bool IsNoOrders { get => isNoOrders; set => SetValue(ref isNoOrders, value); }
        public bool IsOrders { get => isOrders; set => SetValue(ref isOrders, value); }
        public bool IsRequests { get => isRequests; set => SetValue(ref isRequests, value); }

        public Command LoadCollectionsCommand { get; }
        public Command PageAppearingCommand { get; }
        public Command NewReservationCommand { get; }
        public Command NewOrderCommand { get; }
        public Command<DisplayedOrderDto> OrderTappedCommand { get; }
        public Command<StudentRequestDto> AcceptRequestCommand { get; set; }
        public Command<StudentRequestDto> DeclineRequestCommand { get; set; }

        private readonly IReservationService reservationService = DependencyService.Get<IReservationService>();
        private readonly IAdditionalOrderService orderService = DependencyService.Get<IAdditionalOrderService>();
        private readonly IStudentRequestService requestService = DependencyService.Get<IStudentRequestService>();

        public TutorStartPageViewModel()
        {
            Reservations = new ObservableCollection<DisplayedReservationDto>();
            Orders = new ObservableCollection<DisplayedOrderDto>();
            Requests = new ObservableCollection<StudentRequestDto>();

            LoadCollectionsCommand = new Command(async () => await OnLoadCollection());
            PageAppearingCommand = new Command(async () => await OnAppearing());
            NewReservationCommand = new Command(async () => await OnNewReservation());
            NewOrderCommand = new Command(async () => await OnNewOrder());
            OrderTappedCommand = new Command<DisplayedOrderDto>(async (order) => await OnOrderTapped(order));
            AcceptRequestCommand = new Command<StudentRequestDto>(async (request) => await AcceptRequest(request));
            DeclineRequestCommand = new Command<StudentRequestDto>(async (request) => await DeclineRequest(request));
        }

        private async Task OnOrderTapped(DisplayedOrderDto order)
        {
            if (order is null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"//{nameof(OrdersTutorPage)}/{nameof(OrderDetailsTutorPage)}?{nameof(OrderDetailsViewModel.Id)}={order.Id}");
        }

        private async Task OnNewOrder()
        {
            await Shell.Current.GoToAsync($"//{nameof(OrdersTutorPage)}/{nameof(NewOrderTutotPage)}");
        }

        private async Task OnNewReservation()
        {
            await Shell.Current.GoToAsync($"{nameof(NewReservationTutorPage)}?{nameof(NewTutorReservationViewModel.StartDate)}={DateTime.Now.ToShortDateString()}");
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            await SynchronizationService.Instance.StartSynchronization();
        }

        private async Task OnLoadCollection()
        {
            if (IsRefreshing)
            {
                return;
            }

            IsRefreshing = true;
            await LoadReservations();
            await LoadOrders();
            await LoadRequests();
            IsRefreshing = false;
        }

        private async Task LoadReservations()
        {
            IsBusy = true;

            var reservations = await GetReservationsAsync();
            var displayedReservation = reservations.Select(reservation => new DisplayedReservationDto(reservation)).ToList();
            Reservations.Clear();
            displayedReservation.ForEach(reservation => Reservations.Add(reservation));
            IsNoReservations = Reservations.Count == 0;
            IsReservations = !IsNoReservations;

            IsBusy = false;
        }

        private async Task LoadOrders()
        {
            IsBusy = true;

            var orders = await GetOrdersAsync();
            var displayedOrders = orders.Select(order => new DisplayedOrderDto(order)).ToList();
            Orders.Clear();
            displayedOrders.ForEach(order => Orders.Add(order));
            IsOrders = Orders.Any();
            IsNoOrders = !IsOrders;

            IsBusy = false;
        }

        private async Task LoadRequests()
        {
            IsBusy = true;

            var requests = await requestService.GetRequestsByTutorId();
            Requests.Clear();
            requests.ToList().ForEach(request => Requests.Add(request));
            IsRequests = Requests.Any();

            IsBusy = false;
        }

        private async Task<IEnumerable<ReservationDto>> GetReservationsAsync()
        {
            var parameters = GetReservationsParameters();
            var reservation = await reservationService.GetReservationsByTutorAsync(parameters);

            return reservation.Reservations.ToList();
        }

        private async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            var parameters = GetOrdersParameters();
            var orders = await orderService.GetOrdersAsync(parameters);

            return orders.Orders;
        }

        private async Task DeclineRequest(StudentRequestDto request)
        {
            if (request is null)
            {
                return;
            }

            var result = await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.ConfirmationStudentRequestDeletion, GeneralConstans.Yes, GeneralConstans.No);
            if (result)
            {
                await RemoveRequestAsync(request.Id);
            }
        }

        private async Task RemoveRequestAsync(long requestId)
        {
            var removed = await requestService.DeclineRequest(requestId);
            if (removed)
            {
                ToastHelper.MakeLongToast(ToastConstans.StudentRequestRemoved);
                await OnLoadCollection();
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async Task AcceptRequest(StudentRequestDto request)
        {
            if (request is null)
            {
                return;
            }

            await PopupNavigation.Instance.PushAsync(new NewExistingStudentTutorPopupPage(request.StudentId));
        }

        private ReservationParameters GetReservationsParameters()
        {
            return new ReservationParameters
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                IsAtStudent = true,
                IsAtTutor = true,
                IsOnline = true,
                OrderBy = SortingConstans.SortByStartTimeAsc
            };
        }

        private AdditionalOrderParameters GetOrdersParameters()
        {
            return new AdditionalOrderParameters
            {
                IsNotPaid = true,
                IsPaid = true,
                IsPending = true,
                IsInProgress = true,
                IsRealized = false,
                ReceiptStartDate = DateTime.Now.AddYears(-1),
                ReceiptEndDate = DateTime.Now,
                DeadlineStart = DateTime.Now.AddYears(-1),
                DeadlineEnd = DateTime.Now.AddYears(10),
                OrderBy = SortingConstans.SortByDeadlineAsc,
                PageSize = 50
            };
        }
    }
}