using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.ReportViewModels
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
        public Command PageAppearingCommand { get; }
        public Command StudentsReportCommand { get; }
        public Command SubjectsReportCommand { get; }
        public Command SubjectCategoriesReportCommand { get; }
        public Command PlacesReportCommand { get; }

        public MainReportViewModel()
        {
            SetReportDateIntervals();
            PageAppearingCommand = new Command(async () => await OnAppearing());
            StudentsReportCommand = new Command(async () => await OnStudentsReport());
            SubjectsReportCommand = new Command(async () => await OnSubjectsReport());
            SubjectCategoriesReportCommand = new Command(async () => await OnSubjectCategoriesReport());
            PlacesReportCommand = new Command(async () => await OnPlacesReport());
        }

        private async Task OnPlacesReport()
        {
            await Shell.Current.GoToAsync($"{nameof(PlacesReportTutorPage)}");
        }

        private async Task OnSubjectCategoriesReport()
        {
            await Shell.Current.GoToAsync($"{nameof(SubjectCategoriesReportTutorPage)}");
        }

        private async Task OnSubjectsReport()
        {
            await Shell.Current.GoToAsync($"{nameof(SubjectCategoriesReportTutorPage)}");
        }

        private async Task OnStudentsReport()
        {
            await Shell.Current.GoToAsync($"{nameof(StudentsReportTutorPage)}");
        }

        private async void SetReportDateIntervals()
        {
            await SecureStorage.SetAsync("reportStartDate", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString());
            await SecureStorage.SetAsync("reportEndDate", DateTime.Now.ToShortDateString());
        }

        private async Task OnAppearing()
        {
            TutoringProfit = $"1200 zł";
            OrderProfit = $"400 zł";
            TotalProfit = $"1600 zł";
            TotalHours = $"22h";
        }
    }
}
