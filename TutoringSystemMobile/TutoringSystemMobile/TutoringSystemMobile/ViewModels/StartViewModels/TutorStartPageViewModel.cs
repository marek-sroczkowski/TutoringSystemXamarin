using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.AdditionalOrderDtos;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReservationDtos;
using TutoringSystemMobile.Models.StudentRequestDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Synchronization;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.StartViewModels
{
    public class TutorStartPageViewModel : BaseViewModel
    {
        private bool isRefreshing;

        public ObservableCollection<DisplayedReservationDto> Reservations { get; }
        public ObservableCollection<OrderDto> Orders { get; }
        public ObservableCollection<StudentRequestDto> Requests { get; }

        public bool IsRefreshing { get => isRefreshing; set => SetValue(ref isRefreshing, value); }

        public Command LoadCollectionsCommand { get; }
        public Command PageAppearingCommand { get; }

        public TutorStartPageViewModel()
        {
            Reservations = new ObservableCollection<DisplayedReservationDto>();
            Orders = new ObservableCollection<OrderDto>();
            Requests = new ObservableCollection<StudentRequestDto>();
            LoadCollectionsCommand = new Command(async () => await OnLoadCollection());
            PageAppearingCommand = new Command(async () => await OnAppearing());
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

           await SynchronizationService.Instance.StartSynchronization();
        }

        private async Task OnLoadCollection()
        {
            if (IsRefreshing)
                return;

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
            var displayedReservation = reservations.Select(reservation => new DisplayedReservationDto(reservation));
            Reservations.Clear();
            displayedReservation.ToList().ForEach(reservation => Reservations.Add(reservation));

            IsBusy = false;
        }

        private async Task LoadOrders()
        {
            IsBusy = true;

            var orders = await GetOrdersAsync();
            Orders.Clear();
            orders.ForEach(order => Orders.Add(order));

            IsBusy = false;
        }

        private async Task LoadRequests()
        {
            IsBusy = true;

            var requests = await DependencyService.Get<IStudentRequestService>().GetRequestsByTutorId();
            Requests.Clear();
            requests.ToList().ForEach(request => Requests.Add(request));

            IsBusy = false;
        }

        private static async Task<IEnumerable<ReservationDto>> GetReservationsAsync()
        {
            var reservation = await DependencyService.Get<IReservationService>()
                .GetReservationsByTutorAsync(new ReservationParameters
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    IsAtStudent = true,
                    IsAtTutor = true,
                    IsOnline = true,
                    OrderBy = SortingConstans.SortByStartTimeAsc
                });

            return reservation.Reservations.ToList();
        }

        private static async Task<List<OrderDto>> GetOrdersAsync()
        {
            var orders = await DependencyService.Get<IAdditionalOrderService>()
                .GetAdditionalOrdersAsync(new AdditionalOrderParameters
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
                    OrderBy = SortingConstans.SortByReceiptDateDesc
                });

            return orders.Orders.ToList();
        }
    }
}