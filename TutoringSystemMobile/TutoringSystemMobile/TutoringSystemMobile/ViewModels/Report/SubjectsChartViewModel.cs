﻿using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Report;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Report
{
    [QueryProperty(nameof(StartDate), nameof(StartDate))]
    [QueryProperty(nameof(EndDate), nameof(EndDate))]
    [QueryProperty(nameof(IsIncludeZeroProfit), nameof(IsIncludeZeroProfit))]
    public class SubjectsChartViewModel : BaseViewModel
    {
        private DateTime startDate;
        private DateTime endDate;
        private bool isIncludeZeroProfit;
        private Chart donutChart;
        private string selectedDataSource;
        private string selectedChartType;

        public ObservableCollection<SubjectReportDto> SubjectReports { get; }

        public DateTime StartDate { get => startDate; set => SetValue(ref startDate, value); }
        public DateTime EndDate { get => endDate; set => SetValue(ref endDate, value); }
        public bool IsIncludeZeroProfit { get => isIncludeZeroProfit; set => SetValue(ref isIncludeZeroProfit, value); }

        public Command PageAppearingCommand { get; }

        private readonly IReportService reportService;

        public Chart DonutChart { get => donutChart; set => SetValue(ref donutChart, value); }

        public List<string> ChartDataSources { get; set; }
        public List<string> ChartTypes { get; set; }
        public string SelectedDataSource
        {
            get => selectedDataSource;
            set
            {
                SetValue(ref selectedDataSource, value);
                InitData();
            }
        }
        public string SelectedChartType
        {
            get => selectedChartType;
            set
            {
                SetValue(ref selectedChartType, value);
                InitData();
            }
        }

        public SubjectsChartViewModel()
        {
            reportService = DependencyService.Get<IReportService>();
            SubjectReports = new ObservableCollection<SubjectReportDto>();
            PageAppearingCommand = new Command(async () => await OnLoadReport());
            ChartDataSources = new List<string>
            {
                PickerConstans.Profit,
                PickerConstans.ReservationCount,
                PickerConstans.TotalHours
            };
            SelectedDataSource = ChartDataSources[0];
            ChartTypes = new List<string>
            {
                PickerConstans.DonutChart,
                PickerConstans.BarChart
            };
            SelectedChartType = ChartTypes[0];
        }

        private async Task OnLoadReport()
        {
            if (IsBusy || StartDate == default || EndDate == default)
                return;

            IsBusy = true;

            SubjectReports.Clear();
            var reports = await reportService.GetSubjectReportAsync(new ReportParameters(StartDate, EndDate, string.Empty));
            if (!IsIncludeZeroProfit)
                reports = reports.Where(r => r.TotalProfit > 0 && r.TotalHours > 0 && r.ReservationsCount > 0);
            foreach (var report in reports)
                SubjectReports.Add(report);

            InitData();

            IsBusy = false;
        }

        private void InitData()
        {
            IEnumerable<ChartEntry> donutChartEntries;
            switch (SelectedDataSource)
            {
                case PickerConstans.Profit:
                default:
                    donutChartEntries = GetProfitEntries();
                    break;
                case PickerConstans.ReservationCount:
                    donutChartEntries = GetReservationsCountEntries();
                    break;
                case PickerConstans.TotalHours:
                    donutChartEntries = GetHoursEntries();
                    break;
            }

            if (SelectedChartType == PickerConstans.BarChart)
            {
                DonutChart = new BarChart
                {
                    Entries = donutChartEntries,
                    LabelTextSize = 35f,
                    BackgroundColor = SKColor.Empty,
                    ValueLabelOrientation = Orientation.Horizontal
                };
            }
            else if (SelectedChartType == PickerConstans.DonutChart)
            {
                DonutChart = new DonutChart
                {
                    Entries = donutChartEntries,
                    LabelTextSize = 35f,
                    BackgroundColor = SKColor.Empty,
                    LabelMode = LabelMode.RightOnly
                };
            }
        }

        private IEnumerable<ChartEntry> GetProfitEntries()
        {
            var donutChartEntries = new List<ChartEntry>();
            foreach (var report in SubjectReports)
            {
                var label = SelectedChartType == PickerConstans.DonutChart ?
                    report.SubjectName.Length > 17 ? report.SubjectName.Substring(0, 17) : report.SubjectName :
                    report.SubjectName;

                string color = string.Format("#{0:X6}", new Random().Next(0x1000000));

                donutChartEntries.Add(new ChartEntry((float)report.TotalProfit)
                {
                    Color = SKColor.Parse(color),
                    ValueLabel = $"{report.TotalProfit} PLN",
                    Label = label,
                    ValueLabelColor = SKColor.Parse(color)
                });
            }

            return donutChartEntries;
        }

        private IEnumerable<ChartEntry> GetHoursEntries()
        {
            var donutChartEntries = new List<ChartEntry>();
            foreach (var report in SubjectReports)
            {
                var label = SelectedChartType == PickerConstans.DonutChart ?
                   report.SubjectName.Length > 17 ? report.SubjectName.Substring(0, 17) : report.SubjectName :
                   report.SubjectName;

                string color = string.Format("#{0:X6}", new Random().Next(0x1000000));

                donutChartEntries.Add(new ChartEntry((float)report.TotalHours)
                {
                    Color = SKColor.Parse(color),
                    ValueLabel = $"{report.TotalHours}h",
                    Label = label,
                    ValueLabelColor = SKColor.Parse(color)
                });
            }

            return donutChartEntries;
        }

        private IEnumerable<ChartEntry> GetReservationsCountEntries()
        {
            var donutChartEntries = new List<ChartEntry>();
            foreach (var report in SubjectReports)
            {
                var label = SelectedChartType == PickerConstans.DonutChart ?
                   report.SubjectName.Length > 17 ? report.SubjectName.Substring(0, 17) : report.SubjectName :
                   report.SubjectName;

                string color = string.Format("#{0:X6}", new Random().Next(0x1000000));

                donutChartEntries.Add(new ChartEntry(report.ReservationsCount)
                {
                    Color = SKColor.Parse(color),
                    ValueLabel = $"{report.ReservationsCount}",
                    Label = label,
                    ValueLabelColor = SKColor.Parse(color)
                });
            }

            return donutChartEntries;
        }
    }
}