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
    public class SubjectsReportViewModel : BaseViewModel
    {
        private DateTime startDate;
        private DateTime endDate;
        private bool isRefreshing;
        private string sortBy;
        private bool isIncludeZeroProfit = true;

        public ObservableCollection<SubjectSummaryDto> SubjectReports { get; }

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

        private readonly IReportService reportService = DependencyService.Get<IReportService>();

        public SubjectsReportViewModel()
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
            SubjectReports = new ObservableCollection<SubjectSummaryDto>();
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
            await Shell.Current.GoToAsync($"{nameof(SubjectsChartTutorPage)}?{nameof(SubjectsChartViewModel.StartDate)}={StartDate.ToShortDateString()}&{nameof(SubjectsChartViewModel.EndDate)}={EndDate.ToShortDateString()}&{nameof(SubjectsChartViewModel.IsIncludeZeroProfit)}={IsIncludeZeroProfit}");
        }

        private async Task OnLoadReport()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            IsRefreshing = true;
            await GetReportAsync();
            IsBusy = false;
            IsRefreshing = false;
        }

        private async Task GetReportAsync()
        {
            SubjectReports.Clear();

            var reports = await reportService.GetSubjectReportAsync(new ReportParameters(StartDate, EndDate, SortBy));
            if (!IsIncludeZeroProfit)
            {
                reports = reports.Where(r => r.TotalProfit > 0 && r.TotalHours > 0 && r.ReservationsCount > 0);
            }

            var formattedReports = reports.Select(r => new SubjectSummaryDto(r.SubjectName, r.ReservationsCount, r.TotalHours, r.TotalProfit));
            formattedReports.ToList().ForEach(report => SubjectReports.Add(report));
        }
    }
}