using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Helpers;
using TutoringSystemMobile.Models.ReportDtos;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.ReportViewModels
{
    public class ReportSortingViewModel : BaseViewModel
    {
        private bool isSortingByProfitAsc;
        private bool isSortingByProfitDesc = true;
        private bool isSortingByHoursAsc;
        private bool isSortingByHoursDesc;
        private bool isSortingByReservationsCountAsc;
        private bool isSortingByReservationsCountDesc;
        private string sortBy;

        public string SortBy { get => sortBy; set => SetValue(ref sortBy, value); }

        public bool IsSortingByProfitAsc
        {
            get => isSortingByProfitAsc;
            set
            {
                OnSortByProfitAsc();
                isSortingByProfitAsc = value;
            }
        }
        public bool IsSortingByProfitDesc
        {
            get => isSortingByProfitDesc;
            set
            {
                OnSortByProfitDesc();
                isSortingByProfitDesc = value;
            }
        }
        public bool IsSortingByHoursAsc
        {
            get => isSortingByHoursAsc;
            set
            {
                OnSortByHoursAsc();
                isSortingByHoursAsc = value;
            }
        }
        public bool IsSortingByHoursDesc
        {
            get => isSortingByHoursDesc;
            set
            {
                OnSortByHoursDesc();
                isSortingByHoursDesc = value;
            }
        }
        public bool IsSortingByReservationsCountAsc
        {
            get => isSortingByReservationsCountAsc;
            set
            {
                OnSortByReservationsCountAsc();
                isSortingByReservationsCountAsc = value;
            }
        }
        public bool IsSortingByReservationsCountDesc
        {
            get => isSortingByReservationsCountDesc;
            set
            {
                OnSortByReservationsCountDesc();
                isSortingByReservationsCountDesc = value;
            }
        }

        public Command SortByProfitAscCommand { get; }
        public Command SortByProfitDescCommand { get; }
        public Command SortByHoursAscCommand { get; }
        public Command SortByHoursDescCommand { get; }
        public Command SortByReservationsCountAscCommand { get; }
        public Command SortByReservationsCountDescCommand { get; }

        public ReportSortingViewModel(string sortBy)
        {
            SortBy = sortBy;
            OnSetOrderBy();
            SortByProfitAscCommand = new Command(OnSortByProfitAsc);
            SortByProfitDescCommand = new Command(OnSortByProfitDesc);
            SortByHoursAscCommand = new Command(OnSortByHoursAsc);
            SortByHoursDescCommand = new Command(OnSortByHoursDesc);
            SortByReservationsCountAscCommand = new Command(OnSortByReservationsCountAsc);
            SortByReservationsCountDescCommand = new Command(OnSortByReservationsCountDesc);
        }

        private async void OnSortByReservationsCountDesc()
        {
            await SortOrdersAsync(new ReportSortingRadioButtonsActivity(false, false, false, false, false, true));
        }

        private async void OnSortByReservationsCountAsc()
        {
            await SortOrdersAsync(new ReportSortingRadioButtonsActivity(false, false, false, false, true, false));
        }

        private async void OnSortByHoursDesc()
        {
            await SortOrdersAsync(new ReportSortingRadioButtonsActivity(false, false, false, true, false, false));
        }

        private async void OnSortByHoursAsc()
        {
            await SortOrdersAsync(new ReportSortingRadioButtonsActivity(false, false, true, false, false, false));
        }

        private async void OnSortByProfitDesc()
        {
            await SortOrdersAsync(new ReportSortingRadioButtonsActivity(false, true, false, false, false, false));
        }

        private async void OnSortByProfitAsc()
        {
            await SortOrdersAsync(new ReportSortingRadioButtonsActivity(true, false, false, false, false, false));
        }

        private void SetSortingParams()
        {
            if (IsSortingByProfitAsc)
                SortBy = $"{nameof(BaseReportDto.TotalProfit)}";
            else if (IsSortingByProfitDesc)
                SortBy = $"{nameof(BaseReportDto.TotalProfit)} desc";
            else if (IsSortingByHoursAsc)
                SortBy = $"{nameof(BaseReportDto.TotalHours)}";
            else if (IsSortingByHoursDesc)
                SortBy = $"{nameof(BaseReportDto.TotalHours)} desc";
            else if (IsSortingByReservationsCountAsc)
                SortBy = $"{nameof(BaseReportDto.ReservationsCount)}";
            else if (IsSortingByReservationsCountDesc)
                SortBy = $"{nameof(BaseReportDto.ReservationsCount)} desc";
        }

        private void OnSetOrderBy()
        {
            switch (SortBy.ToLower())
            {
                case "totalprofit":
                    SetButtonsActivity(new ReportSortingRadioButtonsActivity(true, false, false, false, false, false));
                    break;
                case "totalprofit desc":
                    SetButtonsActivity(new ReportSortingRadioButtonsActivity(false, true, false, false, false, false));
                    break;
                case "totalhours":
                    SetButtonsActivity(new ReportSortingRadioButtonsActivity(false, false, true, false, false, false));
                    break;
                case "totalhours desc":
                    SetButtonsActivity(new ReportSortingRadioButtonsActivity(false, false, false, true, false, false));
                    break;
                case "reservationscount":
                    SetButtonsActivity(new ReportSortingRadioButtonsActivity(false, false, false, false, true, false));
                    break;
                case "reservationscount desc":
                    SetButtonsActivity(new ReportSortingRadioButtonsActivity(false, false, false, false, false, true));
                    break;
            }
        }

        private async Task SortOrdersAsync(ReportSortingRadioButtonsActivity buttonsActivity)
        {
            if (IsBusy || PopupNavigation.Instance.PopupStack.Count == 0)
                return;

            IsBusy = true;
            SetButtonsActivity(buttonsActivity);
            IsBusy = false;

            SetSortingParams();
            MessagingCenter.Send(this, "reportSorting");

            await PopupNavigation.Instance.PopAsync();
        }

        private void SetButtonsActivity(ReportSortingRadioButtonsActivity buttonsActivity)
        {
            IsSortingByProfitAsc = buttonsActivity.IsSortingByProfitAsc;
            IsSortingByProfitDesc = buttonsActivity.IsSortingByProfitDesc;
            IsSortingByHoursAsc = buttonsActivity.IsSortingByHoursAsc;
            IsSortingByHoursDesc = buttonsActivity.IsSortingByHoursDesc;
            IsSortingByReservationsCountAsc = buttonsActivity.IsSortingByReservationsCountAsc;
            IsSortingByReservationsCountDesc = buttonsActivity.IsSortingByReservationsCountDesc;
        }
    }
}