using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Report;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Report
{
    public class MainReportViewModel : BaseViewModel
    {
        private DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        private DateTime endDate = DateTime.Now;
        private string tutoringProfit;
        private string orderProfit;
        private string totalProfit;
        private string totalHours;

        public DateTime StartDate { get => startDate; set => SetValue(ref startDate, value); }
        public DateTime EndDate { get => endDate; set => SetValue(ref endDate, value); }
        public string TutoringProfit { get => tutoringProfit; set => SetValue(ref tutoringProfit, value); }
        public string OrderProfit { get => orderProfit; set => SetValue(ref orderProfit, value); }
        public string TotalProfit { get => totalProfit; set => SetValue(ref totalProfit, value); }
        public string TotalHours { get => totalHours; set => SetValue(ref totalHours, value); }

        public Command OpenFilteringPopupCommand { get; }
        public Command OpenMainChartCommand { get; }
        public Command PageAppearingCommand { get; }
        public Command StudentsReportCommand { get; }
        public Command SubjectsReportCommand { get; }
        public Command SubjectCategoriesReportCommand { get; }
        public Command PlacesReportCommand { get; }

        private readonly IReportService reportService = DependencyService.Get<IReportService>();

        public MainReportViewModel()
        {
            MessagingCenter.Subscribe<ReportFilteringViewModel>(this, MessagingCenterConstans.FilterByDates, async (sender) =>
            {
                StartDate = sender.StartDate;
                EndDate = sender.EndDate;
                await OnAppearing();
            });

            OpenFilteringPopupCommand = new Command(async () => await OnOpenFiltering());
            OpenMainChartCommand = new Command(async () => await OnOpenMainChart());
            PageAppearingCommand = new Command(async () => await OnAppearing());
            StudentsReportCommand = new Command(async () => await OnStudentsReport());
            SubjectsReportCommand = new Command(async () => await OnSubjectsReport());
            SubjectCategoriesReportCommand = new Command(async () => await OnSubjectCategoriesReport());
            PlacesReportCommand = new Command(async () => await OnPlacesReport());
        }

        private async Task OnOpenFiltering()
        {
            await PopupNavigation.Instance.PushAsync(new ReportFilteringPopupPage(new ReportFilteringDto(StartDate, EndDate, isIncludeZeroProfitVisible: false)));
        }

        private async Task OnOpenMainChart()
        {
            await Shell.Current.GoToAsync($"{nameof(GeneralTimedChartTutorPage)}");
        }

        private async Task OnPlacesReport()
        {
            await Shell.Current.GoToAsync($"{nameof(PlacesReportTutorPage)}?{nameof(StudentsReportViewModel.StartDate)}={StartDate.ToShortDateString()}&{nameof(StudentsReportViewModel.EndDate)}={EndDate.ToShortDateString()}");
        }

        private async Task OnSubjectCategoriesReport()
        {
            await Shell.Current.GoToAsync($"{nameof(SubjectCategoriesReportTutorPage)}?{nameof(StudentsReportViewModel.StartDate)}={StartDate.ToShortDateString()}&{nameof(StudentsReportViewModel.EndDate)}={EndDate.ToShortDateString()}");
        }

        private async Task OnSubjectsReport()
        {
            await Shell.Current.GoToAsync($"{nameof(SubjectsReportTutorPage)}?{nameof(StudentsReportViewModel.StartDate)}={StartDate.ToShortDateString()}&{nameof(StudentsReportViewModel.EndDate)}={EndDate.ToShortDateString()}");
        }

        private async Task OnStudentsReport()
        {
            await Shell.Current.GoToAsync($"{nameof(StudentsReportTutorPage)}?{nameof(StudentsReportViewModel.StartDate)}={StartDate.ToShortDateString()}&{nameof(StudentsReportViewModel.EndDate)}={EndDate.ToShortDateString()}");
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            var generalSummary = await reportService.GetGeneralReportAsync(new ReportParameters(StartDate, EndDate, SortingConstans.SortByTotalProfitDesc));
            TutoringProfit = $"{generalSummary.TutoringProfit} zł";
            OrderProfit = $"{generalSummary.OrderProfit} zł";
            TotalProfit = $"{generalSummary.TotalProfit} zł";
            TotalHours = $"{generalSummary.TotalHours}h";

            IsBusy = false;
        }
    }
}