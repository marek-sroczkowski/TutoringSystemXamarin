using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Dtos.Report;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Report
{
    public class ReportFilteringViewModel : BaseViewModel
    {
        private DateTime startDate;
        private DateTime endDate;
        private bool isIncludeZeroProfit;
        private bool isIncludeZeroProfitVisible;

        public DateTime StartDate { get => startDate; set => SetValue(ref startDate, value); }
        public DateTime EndDate { get => endDate; set => SetValue(ref endDate, value); }
        public bool IsIncludeZeroProfit { get => isIncludeZeroProfit; set => SetValue(ref isIncludeZeroProfit, value); }
        public bool IsIncludeZeroProfitVisible { get => isIncludeZeroProfitVisible; set => SetValue(ref isIncludeZeroProfitVisible, value); }

        public Command FilterCommand { get; }

        public ReportFilteringViewModel(ReportFilteringDto reportFiltering)
        {
            StartDate = reportFiltering.StartDate;
            EndDate = reportFiltering.EndDate;
            IsIncludeZeroProfit = reportFiltering.IsIncludeZeroProfit;
            IsIncludeZeroProfitVisible = reportFiltering.IsIncludeZeroProfitVisible;
            FilterCommand = new Command(async () => await OnFilter());
        }

        private async Task OnFilter()
        {
            MessagingCenter.Send(this, MessagingCenterConstans.FilterByDates);
            await PopupNavigation.Instance.PopAsync();
        }
    }
}