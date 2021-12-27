using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReservationDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Models;

namespace TutoringSystemMobile.ViewModels.ReservationViewModels
{
    public class TutorReservationsViewModel : BaseViewModel
    {
        private List<string> months;

        private DateTime selectedDate = DateTime.Now;
        private int year = DateTime.Now.Year;
        private int month = DateTime.Now.Month;
        private string monthLabel;
        private bool isRefreshing;

        public DateTime SelectedDate { get => selectedDate; set => SetValue(ref selectedDate, value); }
        public EventCollection Reservations { get; set; }
        public CultureInfo Culture => new CultureInfo("pl-PL");
        public int Year { get => year; set => SetValue(ref year, value); }
        public int Month
        {
            get => month;
            set
            {
                int newMonth;
                if (value > 12)
                {
                    newMonth = 1;
                    Year++;
                }
                else if (value < 1)
                {
                    newMonth = 12;
                    Year--;
                }
                else
                {
                    newMonth = value;
                }
                MonthLabel = months[newMonth - 1];
                SetValue(ref month, newMonth);
            }
        }
        public string MonthLabel { get => monthLabel; set => SetValue(ref monthLabel, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetValue(ref isRefreshing, value); }

        public Command LoadReservationCommand { get; }
        public Command PageAppearingCommand { get; }
        public Command PrevYearCommand { get; }
        public Command NextYearCommand { get; }
        public Command PrevMonthCommand { get; }
        public Command NextMonthCommand { get; }
        public Command<DisplayedSimpleReservationDto> ReservationTappedCommand { get; }

        public TutorReservationsViewModel()
        {
            SetMonths();
            MonthLabel = months[Month - 1];
            Reservations = new EventCollection();
            LoadReservationCommand = new Command(async () => await OnLoadReservations());
            PageAppearingCommand = new Command(OnAppearing);
            PrevYearCommand = new Command(async () => await OnPrevYear());
            NextYearCommand = new Command(async () => await OnNextYear());
            PrevMonthCommand = new Command(async () => await OnPrevMonth());
            NextMonthCommand = new Command(async () => await OnNextMonth());
            ReservationTappedCommand = new Command<DisplayedSimpleReservationDto>(async (reservation) => await OnReservationTapped(reservation));
        }

        private async Task OnReservationTapped(DisplayedSimpleReservationDto reservation)
        {
            if (reservation is null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ReservationDetailsTutorPage)}?{nameof(TutorReservationDetailsViewModel.Id)}={reservation.Id}");
        }

        private async Task OnNextMonth()
        {
            Month++;
            await OnLoadReservations();
        }

        private async Task OnPrevMonth()
        {
            Month--;
            await OnLoadReservations();
        }

        private async Task OnNextYear()
        {
            Year++;
            await OnLoadReservations();
        }

        private async Task OnPrevYear()
        {
            Year--;
            await OnLoadReservations();
        }

        private void OnAppearing()
        {
            IsBusy = true;
        }

        private async Task OnLoadReservations()
        {
            if (IsRefreshing)
                return;

            IsBusy = true;
            IsRefreshing = true;

            var reservations = await GetReservationsAsync();
            var displayedReservation = reservations.Select(reservation => new DisplayedSimpleReservationDto(reservation)).ToList();
            Reservations.Clear();
            reservations.ToList().ForEach(reservation => AddReservation(reservation, reservations));

            IsRefreshing = false;
            IsBusy = false;
        }

        private void AddReservation(ReservationDto reservation, IEnumerable<ReservationDto> reservations)
        {
            if (!Reservations.ContainsKey(reservation.StartTime.Date))
            {
                Reservations.Add(reservation.StartTime.Date, reservations.Where(r => r.StartTime.Date.Equals(reservation.StartTime.Date))
                    .Select(reservation => new DisplayedSimpleReservationDto(reservation)).ToList());
            }
        }

        private async Task<IEnumerable<ReservationDto>> GetReservationsAsync()
        {
            var reservation = await DependencyService.Get<IReservationService>()
                .GetReservationsByTutorAsync(new ReservationParameters
                {
                    StartDate = new DateTime(Year, Month, 1),
                    EndDate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month)),
                    IsAtStudent = true,
                    IsAtTutor = true,
                    IsOnline = true,
                    OrderBy = SortingConstans.SortByStartTimeAsc
                });

            return reservation.Reservations;
        }

        private void SetMonths()
        {
            months = new List<string>
            {
                PickerConstans.January,
                PickerConstans.February,
                PickerConstans.March,
                PickerConstans.April,
                PickerConstans.May,
                PickerConstans.June,
                PickerConstans.July,
                PickerConstans.August,
                PickerConstans.September,
                PickerConstans.October,
                PickerConstans.November,
                PickerConstans.December
            };
        }
    }
}