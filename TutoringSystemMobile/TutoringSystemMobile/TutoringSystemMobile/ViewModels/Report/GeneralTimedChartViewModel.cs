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
using TutoringSystemMobile.Helpers;
using System.Threading.Tasks;
using System.Linq;

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

        private readonly IReportService reportService = DependencyService.Get<IReportService>();

        public GeneralTimedChartViewModel()
        {
            Months = DateTimeHelper.GetMonthNames();
            SetYears();
            GeneralTimedReport = new ObservableCollection<GeneralTimedReportDto>();
            ChartDataSources = ChartHelper.GetProfitNames();
            PageAppearingCommand = new Command(OnLoadReport);
        }

        private async void OnLoadReport()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            await GetReportAsync();
            InitData();
            IsBusy = false;
        }

        private async Task GetReportAsync()
        {
            GeneralTimedReport.Clear();
            var reports = await reportService.GetGeneralTimedReport(GetReportParameters());
            reports.ToList().ForEach(report => GeneralTimedReport.Add(report));
        }

        private void InitData()
        {
            var entries = GetEntries();

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
            IEnumerable<ChartEntry> entries = SelectedDataSource switch
            {
                PickerConstans.TutoringProfit => GetTutoringProfitEntries(),
                PickerConstans.OrderProfit => GetOrdersProfitEntries(),
                PickerConstans.TotalHours => GetTotalHoursEntries(),
                _ => GetTotalProfitEntries(),
            };

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
            for (int i = DateTime.Now.Year - 6; i <= DateTime.Now.Year; i++)
            {
                Years.Add(i);
            }
        }
    }
}
