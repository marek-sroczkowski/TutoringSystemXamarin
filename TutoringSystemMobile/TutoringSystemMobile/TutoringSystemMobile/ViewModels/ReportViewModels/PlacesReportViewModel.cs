﻿using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.ReportDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.ReportViewModels
{
    [QueryProperty(nameof(StartDate), nameof(StartDate))]
    [QueryProperty(nameof(EndDate), nameof(EndDate))]
    public class PlacesReportViewModel : BaseViewModel
    {
        private DateTime startDate;
        private DateTime endDate;
        private bool isRefreshing;
        private string sortBy;
        private bool isIncludeZeroProfit = true;

        public ObservableCollection<PlaceSummaryDto> PlaceReport { get; }

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


        public PlacesReportViewModel()
        {
            MessagingCenter.Subscribe<ReportFilteringViewModel>(this, "filterByDates", async (sender) =>
            {
                StartDate = sender.StartDate;
                EndDate = sender.EndDate;
                IsIncludeZeroProfit = sender.IsIncludeZeroProfit;
                await OnLoadReport();
            });
            MessagingCenter.Subscribe<BaseReportSortingViewModel>(this, "reportSorting", async (sender) =>
            {
                SortBy = sender.SortBy;
                await OnLoadReport();
            });

            SortBy = $"{nameof(BaseReportDto.TotalProfit)} desc";
            reportService = DependencyService.Get<IReportService>();
            PlaceReport = new ObservableCollection<PlaceSummaryDto>();
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
            await Shell.Current.GoToAsync($"{nameof(PlacesChartTutorPage)}?{nameof(PlacesChartViewModel.StartDate)}={StartDate.ToShortDateString()}&{nameof(PlacesChartViewModel.EndDate)}={EndDate.ToShortDateString()}&{nameof(PlacesChartViewModel.IsIncludeZeroProfit)}={IsIncludeZeroProfit}");
        }

        private async Task OnLoadReport()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsRefreshing = true;

            PlaceReport.Clear();
            var reports = await reportService.GetPlaceReportAsync(new ReportParameters(StartDate, EndDate, SortBy));
            if (!IsIncludeZeroProfit)
                reports = reports.Where(r => r.TotalProfit > 0 && r.TotalHours > 0 && r.ReservationsCount > 0);
            var formattedReports = reports.Select(r => new PlaceSummaryDto(r.Place, r.ReservationsCount, r.TotalHours, r.TotalProfit));
            foreach (var report in formattedReports)
                PlaceReport.Add(report);

            IsBusy = false;
            IsRefreshing = false;
        }
    }
}
