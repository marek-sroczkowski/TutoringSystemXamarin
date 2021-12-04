using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AdditionalOrderDtos;
using TutoringSystemMobile.Models.Helpers;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.OrderViewModels
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
                SortBy = $"{nameof(OrderDetailsDto.Name)}";
            else if (IsSortingByNameDesc)
                SortBy = $"{nameof(OrderDetailsDto.Name)} desc";
            else if (IsSortingByPriceAsc)
                SortBy = $"{nameof(OrderDetailsDto.Cost)}";
            else if (IsSortingByPriceDesc)
                SortBy = $"{nameof(OrderDetailsDto.Cost)} desc";
            else if (IsSortingByDeadlineAsc)
                SortBy = $"{nameof(OrderDetailsDto.Deadline)}";
            else if (IsSortingByDeadlineDesc)
                SortBy = $"{nameof(OrderDetailsDto.Deadline)} desc";
            else if (IsSortingByCreatedDateAsc)
                SortBy = $"{nameof(OrderDetailsDto.ReceiptDate)}";
            else if (IsSortingByCreatedDateDesc)
                SortBy = $"{nameof(OrderDetailsDto.ReceiptDate)} desc";
        }

        private void OnSetOrderBy()
        {
            switch (SortBy.ToLower())
            {
                case "name":
                default:
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(true, false, false, false, false, false, false, false));
                    break;
                case "name desc":
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, true, false, false, false, false, false, false));
                    break;
                case "cost":
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, false, true, false, false, false, false, false));
                    break;
                case "cost desc":
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, false, false, true, false, false, false, false));
                    break;
                case "deadline":
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, false, false, false, true, false, false, false));
                    break;
                case "deadline desc":
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, false, false, false, false, true, false, false));
                    break;
                case "receiptdate":
                    SetButtonsActivity(new OrderSortingRadioButtonsActivity(false, false, false, false, false, false, true, false));
                    break;
                case "receiptdate desc":
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
            if (IsBusy || PopupNavigation.Instance.PopupStack.Count == 0)
                return;

            IsBusy = true;
            SetButtonsActivity(buttonsActivity);
            IsBusy = false;

            SetSortingParams();
            MessagingCenter.Send(this, "orderSorting");

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