using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Reservation;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.ViewModels.Student;
using TutoringSystemMobile.Views;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Models;

namespace TutoringSystemMobile.ViewModels.Reservation
{
    public class StudentReservationsViewModel : BaseViewModel
    {
        private List<string> months;

        private DateTime selectedDate = DateTime.Now;
        private int year = DateTime.Now.Year;
        private int month = DateTime.Now.Month;
        private string monthLabel;
        private bool isRefreshing;

        public DateTime SelectedDate { get => selectedDate; set => SetValue(ref selectedDate, value); }
        public EventCollection Reservations { get; set; }
        public List<ReservationDto> RepeatedReservations { get; set; }
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
        public Command AddReservationCommand { get; }
        public Command<DisplayedSimpleReservationDto> ReservationTappedCommand { get; }

        public StudentReservationsViewModel()
        {
            SetMonths();
            MonthLabel = months[Month - 1];
            Reservations = new EventCollection();
            LoadReservationCommand = new Command(async () => await OnLoadReservations());
            PageAppearingCommand = new Command(async () => await OnAppearing());
            PrevYearCommand = new Command(async () => await OnPrevYear());
            NextYearCommand = new Command(async () => await OnNextYear());
            PrevMonthCommand = new Command(async () => await OnPrevMonth());
            NextMonthCommand = new Command(async () => await OnNextMonth());
            AddReservationCommand = new Command(async () => await OnAddReservation());
            ReservationTappedCommand = new Command<DisplayedSimpleReservationDto>(async (reservation) => await OnReservationTapped(reservation));
        }

        private async Task OnAddReservation()
        {
            await Shell.Current.GoToAsync($"{nameof(NewReservationStudentPage)}?{nameof(NewStudentReservationViewModel.StartDate)}={SelectedDate}");
        }

        private async Task OnReservationTapped(DisplayedSimpleReservationDto reservation)
        {
            if (reservation is null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ReservationDetailsStudentPage)}?{nameof(StudentReservationDetailsViewModel.Id)}={reservation.Id}&{nameof(StudentReservationDetailsViewModel.StartTime)}={reservation.StartTime.ToShortTimeString()}&{nameof(StudentReservationDetailsViewModel.StartDate)}={reservation.StartTime.ToShortDateString()}");
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

        private async Task OnAppearing()
        {
            RepeatedReservations = await GetRepeatedReservationsAsync();
            IsBusy = true;
        }

        private async Task OnLoadReservations()
        {
            if (IsRefreshing)
                return;

            IsBusy = true;
            IsRefreshing = true;

            var reservations = await GetSingleReservationsAsync();
            RepeatedReservations.ForEach(reservation => AddRecurringReservations(reservation, reservations));
            Reservations.Clear();
            reservations.ForEach(reservation => AddReservation(reservation, reservations));

            IsRefreshing = false;
            IsBusy = false;
        }

        private void AddRecurringReservations(ReservationDto reservation, List<ReservationDto> reservations)
        {
            if (reservations.FirstOrDefault(r => r.RepeatedReservationId.Equals(reservation.RepeatedReservationId)) != null)
                return;

            int i = 0;
            var endDate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month)).AddDays(14).Date;
            while (reservation.StartTime.AddDays((int)reservation.Frequency * i).Date <= endDate)
            {
                reservations.Add(new ReservationDto(reservation)
                {
                    StartTime = reservation.StartTime.AddDays((int)reservation.Frequency * i)
                });
                i++;
            }
        }

        private void AddReservation(ReservationDto reservation, IEnumerable<ReservationDto> reservations)
        {
            if (!Reservations.ContainsKey(reservation.StartTime.Date))
            {
                Reservations.Add(reservation.StartTime.Date, reservations.Where(r => r.StartTime.Date.Equals(reservation.StartTime.Date))
                    .Select(reservation => new DisplayedSimpleReservationDto(reservation)).ToList());
            }
        }

        private async Task<List<ReservationDto>> GetSingleReservationsAsync()
        {
            var reservationCollection = await DependencyService.Get<ISingleReservationService>()
                .GetReservationsByStudentAsync(GetReservationParameters());

            return reservationCollection.Reservations.ToList();
        }

        private async Task<List<ReservationDto>> GetRepeatedReservationsAsync()
        {
            var reservations = await DependencyService.Get<IRepeatedReservationService>()
                .GetReservationsByStudentAsync();

            return reservations.Select(reservation => new ReservationDto(reservation)).ToList();
        }

        private ReservationParameters GetReservationParameters()
        {
            return new ReservationParameters
            {
                StartDate = new DateTime(Year, Month, 1).AddDays(-7),
                EndDate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month)).AddDays(7),
                IsAtStudent = true,
                IsAtTutor = true,
                IsOnline = true,
                OrderBy = SortingConstans.SortByStartTimeAsc
            };
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
