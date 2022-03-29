using Rg.Plugins.Popup.Services;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Helpers;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Order
{
    public class OrderSortingViewModel : BaseViewModel
    {
        private string sortBy;
        private bool isSortingByNameAsc;
        private bool isSortingByNameDesc;
        private bool isSortingByPriceAsc;
        private bool isSortingByPriceDesc;
        private bool isSortingByDeadlineAsc;
        private bool isSortingByDeadlineDesc;
        private bool isSortingByCreatedDateAsc;
        private bool isSortingByCreatedDateDesc;

        public string SortBy { get => sortBy; set => SetValue(ref sortBy, value); }

        public bool IsSortingByNameAsc
        {
            get => isSortingByNameAsc;
            set
            {
                OnSortByNameAsc();
                SetValue(ref isSortingByNameAsc, value);
            }
        }
        public bool IsSortingByNameDesc
        {
            get => isSortingByNameDesc;
            set
            {
                OnSortByNameDesc();
                SetValue(ref isSortingByNameDesc, value);
            }
        }
        public bool IsSortingByPriceAsc
        {
            get => isSortingByPriceAsc;
            set
            {
                OnSortByPriceAsc();
                SetValue(ref isSortingByPriceAsc, value);
            }
        }
        public bool IsSortingByPriceDesc
        {
            get => isSortingByPriceDesc;
            set
            {
                OnSortByPriceDesc();
                SetValue(ref isSortingByPriceDesc, value);
            }
        }
        public bool IsSortingByDeadlineAsc
        {
            get => isSortingByDeadlineAsc;
            set
            {
                OnSortByDeadlineAsc();
                SetValue(ref isSortingByDeadlineAsc, value);
            }
        }
        public bool IsSortingByDeadlineDesc
        {
            get => isSortingByDeadlineDesc;
            set
            {
                OnSortByDeadlineDesc();
                SetValue(ref isSortingByDeadlineDesc, value);
            }
        }
        public bool IsSortingByCreatedDateAsc
        {
            get => isSortingByCreatedDateAsc;
            set
            {
                OnSortByCreatedDateAsc();
                SetValue(ref isSortingByCreatedDateAsc, value);
            }
        }
        public bool IsSortingByCreatedDateDesc
        {
            get => isSortingByCreatedDateDesc;
            set
            {
                OnSortByCreatedDateDesc();
                SetValue(ref isSortingByCreatedDateDesc, value);
            }
        }

        public Command SortByNameAscCommand { get; }
        public Command SortByNameDescCommand { get; }
        public Command SortByPriceAscCommand { get; }
        public Command SortByPriceDescCommand { get; }
        public Command SortByDeadlineAscCommand { get; }
        public Command SortByDeadlineDescCommand { get; }
        public Command SortByCreatedDateAscCommand { get; }
        public Command SortByCreatedDateDescCommand { get; }

        public OrderSortingViewModel(string sortBy)
        {
            SortBy = sortBy;
            OnSetOrderBy();
            SortByNameAscCommand = new Command(OnSortByNameAsc);
            SortByNameDescCommand = new Command(OnSortByNameDesc);
            SortByPriceAscCommand = new Command(OnSortByPriceAsc);
            SortByPriceDescCommand = new Command(OnSortByPriceDesc);
            SortByDeadlineAscCommand = new Command(OnSortByDeadlineAsc);
            SortByDeadlineDescCommand = new Command(OnSortByDeadlineDesc);
            SortByCreatedDateAscCommand = new Command(OnSortByCreatedDateAsc);
            SortByCreatedDateDescCommand = new Command(OnSortByCreatedDateDesc);
        }

        private void SetSortingParams()
        {
            if (IsSortingByNameAsc)
            {
                SortBy = SortingConstans.SortByNameAsc;
            }
            else if (IsSortingByNameDesc)
            {
                SortBy = SortingConstans.SortByNameDesc;
            }
            else if (IsSortingByPriceAsc)
            {
                SortBy = SortingConstans.SortByCostAsc;
            }
            else if (IsSortingByPriceDesc)
            {
                SortBy = SortingConstans.SortByCostDesc;
            }
            else if (IsSortingByDeadlineAsc)
            {
                SortBy = SortingConstans.SortByDeadlineAsc;
            }
            else if (IsSortingByDeadlineDesc)
            {
                SortBy = SortingConstans.SortByDeadlineDesc;
            }
            else if (IsSortingByCreatedDateAsc)
            {
                SortBy = SortingConstans.SortByReceiptDateAsc;
            }
            else if (IsSortingByCreatedDateDesc)
            {
                SortBy = SortingConstans.SortByReceiptDateDesc;
            }
        }

        private void OnSetOrderBy()
        {
            switch (SortBy.ToLower())
            {
                case SortingConstans.SortByNameAsc:
                default:
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(true, false, false, false, false, false, false, false));
                    break;
                case SortingConstans.SortByNameDesc:
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, true, false, false, false, false, false, false));
                    break;
                case SortingConstans.SortByCostAsc:
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, false, true, false, false, false, false, false));
                    break;
                case SortingConstans.SortByCostDesc:
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, false, false, true, false, false, false, false));
                    break;
                case SortingConstans.SortByDeadlineAsc:
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, false, false, false, true, false, false, false));
                    break;
                case SortingConstans.SortByDeadlineDesc:
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, false, false, false, false, true, false, false));
                    break;
                case SortingConstans.SortByReceiptDateAsc:
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, false, false, false, false, false, true, false));
                    break;
                case SortingConstans.SortByReceiptDateDesc:
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, false, false, false, false, false, false, true));
                    break;
            }
        }

        private async void OnSortByCreatedDateDesc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, false, false, false, false, false, false, true));
        }

        private async void OnSortByCreatedDateAsc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, false, false, false, false, false, true, false));
        }

        private async void OnSortByDeadlineDesc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, false, false, false, false, true, false, false));
        }

        private async void OnSortByDeadlineAsc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, false, false, false, true, false, false, false));
        }

        private async void OnSortByPriceDesc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, false, false, true, false, false, false, false));
        }

        private async void OnSortByPriceAsc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, false, true, false, false, false, false, false));
        }

        private async void OnSortByNameDesc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(false, true, false, false, false, false, false, false));
        }

        private async void OnSortByNameAsc()
        {
            await SortOrdersAsync(new OrderSortingRadioButtonsActivity(true, false, false, false, false, false, false, false));
        }

        private async Task SortOrdersAsync(OrderSortingRadioButtonsActivity buttonsActivity)
        {
            if (IsBusy || !PopupNavigation.Instance.PopupStack.Any())
            {
                return;
            }

            IsBusy = true;
            SetButtonsActivity(buttonsActivity);
            IsBusy = false;

            SetSortingParams();
            MessagingCenter.Send(this, MessagingCenterConstans.OrderSorting);

            await PopupNavigation.Instance.PopAsync();
        }

        private void SetButtonsActivity(OrderSortingRadioButtonsActivity buttonsActivity)
        {
            IsSortingByNameAsc = buttonsActivity.IsSortingByNameAsc;
            IsSortingByNameDesc = buttonsActivity.IsSortingByNameDesc;
            IsSortingByPriceAsc = buttonsActivity.IsSortingByPriceAsc;
            IsSortingByPriceDesc = buttonsActivity.IsSortingByPriceDesc;
            IsSortingByDeadlineAsc = buttonsActivity.IsSortingByDeadlineAsc;
            IsSortingByDeadlineDesc = buttonsActivity.IsSortingByDeadlineDesc;
            IsSortingByCreatedDateAsc = buttonsActivity.IsSortingByCreatedDateAsc;
            IsSortingByCreatedDateDesc = buttonsActivity.IsSortingByCreatedDateDesc;
        }
    }
}