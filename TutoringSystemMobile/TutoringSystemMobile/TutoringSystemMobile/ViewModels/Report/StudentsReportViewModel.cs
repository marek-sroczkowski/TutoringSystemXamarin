using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Report;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Report
{
    [QueryProperty(nameof(StartDate), nameof(StartDate))]
    [QueryProperty(nameof(EndDate), nameof(EndDate))]
    public class StudentsReportViewModel : BaseViewModel
    {
        private DateTime startDate;
        private DateTime endDate;
        private bool isRefreshing;
        private string sortBy;
        private bool isIncludeZeroProfit = true;

        public ObservableCollection<StudentSummaryDto> StudentReports { get; }

        public DateTime StartDate { get => startDate; set => SetValue(ref startDate, value); }
        public DateTime EndDate { get => endDate; set => SetValue(ref endDate, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetValue(ref isRefreshing, value); }
        public bool IsIncludeZeroProfit { get => isIncludeZeroProfit; set => SetValue(ref isIncludeZeroProfit, value); }
        public string SortBy { get => sortBy; set => SetValue(ref sortBy, value); }

        public Command LoadReportCommand { get; }
        public Command OpenFilteringPopupCommand { get; }
        public Command OpenSortingPopupCommand { get; }
        public Command OpenStudentsChartCommand { get; }
        public Command PageAppearingCommand { get; }

        private readonly IReportService reportService;

        public StudentsReportViewModel()
        {
            MessagingCenter.Subscribe<ReportFilteringViewModel>(this, MessagingCenterConstans.FilterByDates, async (sender) =>
            {
                StartDate = sender.StartDate;
                EndDate = sender.EndDate;
                IsIncludeZeroProfit = sender.IsIncludeZeroProfit;
                await OnLoadReport();
            });
            MessagingCenter.Subscribe<ReportSortingViewModel>(this, MessagingCenterConstans.ReportSorting, async (sender) =>
            {
                SortBy = sender.SortBy;
                await OnLoadReport();
            });

            SortBy = SortingConstans.SortByTotalProfitDesc;
            reportService = DependencyService.Get<IReportService>();
            StudentReports = new ObservableCollection<StudentSummaryDto>();
            LoadReportCommand = new Command(async () => await OnLoadReport());
            OpenFilteringPopupCommand = new Command(async () => await OnOpenFilteringPopup());
            OpenSortingPopupCommand = new Command(async () => await OnOpenSortingPopup());
            OpenStudentsChartCommand = new Command(async () => await OnOpenStudentsChart());
            PageAppearingCommand = new Command(OnAppearing);
        }

        private void OnAppearing()
        {
            IsRefreshing = true;
        }

        private async Task OnOpenFilteringPopup()
        {
            await PopupNavigation.Instance.PushAsync(new ReportFilteringPopupPage(new ReportFilteringDto(StartDate, EndDate, IsIncludeZeroProfit)));
        }

        private async Task OnOpenSortingPopup()
        {
            await PopupNavigation.Instance.PushAsync(new ReportSortingPopupPage(SortBy));
        }

        private async Task OnOpenStudentsChart()
        {
            await Shell.Current.GoToAsync($"{nameof(StudentsChartTutorPage)}?{nameof(StudentsChartViewModel.StartDate)}={StartDate.ToShortDateString()}&{nameof(StudentsChartViewModel.EndDate)}={EndDate.ToShortDateString()}&{nameof(StudentsChartViewModel.IsIncludeZeroProfit)}={IsIncludeZeroProfit}");
        }

        private async Task OnLoadReport()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsRefreshing = true;

            StudentReports.Clear();
            var reports = await reportService.GetStudentsReportAsync(new ReportParameters(StartDate, EndDate, SortBy));
            if (!IsIncludeZeroProfit)
                reports = reports.Where(r => r.TotalProfit > 0 && r.TotalHours > 0 && r.ReservationsCount > 0);
            var formattedReports = reports.Select(r => new StudentSummaryDto(r.Username, r.StudentName, r.ReservationsCount, r.TotalHours, r.TotalProfit));
            foreach (var report in formattedReports)
                StudentReports.Add(report);

            IsBusy = false;
            IsRefreshing = false;
        }
    }
}