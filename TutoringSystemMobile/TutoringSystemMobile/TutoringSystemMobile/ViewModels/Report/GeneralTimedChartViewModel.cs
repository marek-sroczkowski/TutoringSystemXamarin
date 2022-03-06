using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Report;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Report
{
    public class GeneralTimedChartViewModel : BaseViewModel
    {
        private int selectedYear = DateTime.Now.Year;
        private string selectedDataSource = PickerConstans.TotalProfit;
        private Chart chart;

        public ObservableCollection<GeneralTimedReportDto> GeneralTimedReport { get; }

        public int SelectedYear
        {
            get => selectedYear;
            set
            {
                SetValue(ref selectedYear, value);
                OnLoadReport();
            }
        }
        public string SelectedDataSource
        {
            get => selectedDataSource;
            set
            {
                SetValue(ref selectedDataSource, value);
                InitData();
            }
        }

        public List<string> Months { get; set; }
        public List<int> Years { get; set; }
        public List<string> ChartDataSources { get; set; }

        public Command PageAppearingCommand { get; }

        public Chart Chart { get => chart; set => SetValue(ref chart, value); }

        public GeneralTimedChartViewModel()
        {
            GeneralTimedReport = new ObservableCollection<GeneralTimedReportDto>();
            SetMonths();
            SetYears();
            SetChartDataSource();
            PageAppearingCommand = new Command(OnLoadReport);
        }

        private async void OnLoadReport()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            var reports = await DependencyService.Get<IReportService>().GetGeneralTimedReport(GetReportParameters());
            GeneralTimedReport.Clear();
            foreach (var report in reports)
            {
                GeneralTimedReport.Add(report);
            }

            InitData();

            IsBusy = false;
        }

        private void InitData()
        {
            IEnumerable<ChartEntry> entries = GetEntries();

            Chart = new LineChart
            {
                Entries = entries,
                LabelTextSize = 30f,
                BackgroundColor = SKColor.Empty,
                LabelOrientation = Orientation.Horizontal
            };
        }

        private IEnumerable<ChartEntry> GetEntries()
        {
            IEnumerable<ChartEntry> entries;

            switch (SelectedDataSource)
            {
                case PickerConstans.TotalProfit:
                default:
                    entries = GetTotalProfitEntries();
                    break;
                case PickerConstans.TutoringProfit:
                    entries = GetTutoringProfitEntries();
                    break;
                case PickerConstans.OrderProfit:
                    entries = GetOrdersProfitEntries();
                    break;
                case PickerConstans.TotalHours:
                    entries = GetTotalHoursEntries();
                    break;
            }

            return entries;
        }

        private IEnumerable<ChartEntry> GetTotalHoursEntries()
        {
            var entries = new List<ChartEntry>();
            for (int i = 0; i < GeneralTimedReport.Count; i++)
            {
                entries.Add(new ChartEntry((float)GeneralTimedReport[i].Report.TotalHours)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabelColor = SKColor.Parse("#09C"),
                    ValueLabel = $"{GeneralTimedReport[i].Report.TotalHours}h",
                    Label = Months[i]
                });
            }

            return entries;
        }

        private List<ChartEntry> GetOrdersProfitEntries()
        {
            var entries = new List<ChartEntry>();
            for (int i = 0; i < GeneralTimedReport.Count; i++)
            {
                entries.Add(new ChartEntry((float)GeneralTimedReport[i].Report.OrderProfit)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabelColor = SKColor.Parse("#09C"),
                    ValueLabel = $"{GeneralTimedReport[i].Report.OrderProfit} PLN",
                    Label = Months[i]
                });
            }

            return entries;
        }

        private List<ChartEntry> GetTutoringProfitEntries()
        {
            var entries = new List<ChartEntry>();
            for (int i = 0; i < GeneralTimedReport.Count; i++)
            {
                entries.Add(new ChartEntry((float)GeneralTimedReport[i].Report.TutoringProfit)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabelColor = SKColor.Parse("#09C"),
                    ValueLabel = $"{GeneralTimedReport[i].Report.TutoringProfit} PLN",
                    Label = Months[i]
                });
            }

            return entries;
        }

        private List<ChartEntry> GetTotalProfitEntries()
        {
            var entries = new List<ChartEntry>();
            for (int i = 0; i < GeneralTimedReport.Count; i++)
            {
                entries.Add(new ChartEntry((float)GeneralTimedReport[i].Report.TotalProfit)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabelColor = SKColor.Parse("#09C"),
                    ValueLabel = $"{GeneralTimedReport[i].Report.TotalProfit} PLN",
                    Label = Months[i]
                });
            }

            return entries;
        }

        private ReportParameters GetReportParameters()
        {
            return new ReportParameters
            {
                StartDate = new DateTime(SelectedYear, 1, 1),
                EndDate = new DateTime(SelectedYear, 12, 31),
                OrderBy = string.Empty
            };
        }

        private void SetYears()
        {
            Years = new List<int>();
            for (int i = 2015; i <= DateTime.Now.Year; i++)
                Years.Add(i);
        }

        private void SetChartDataSource()
        {
            ChartDataSources = new List<string>
            {
                PickerConstans.TotalProfit,
                PickerConstans.TutoringProfit,
                PickerConstans.OrderProfit,
                PickerConstans.TotalHours
            };
        }


        private void SetMonths()
        {
            Months = new List<string>
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
                PickerConstans.October
            };
        }
    }
}
