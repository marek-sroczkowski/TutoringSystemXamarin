using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Services.Utils;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.OrderViewModels
{
    public class OrderFilteringViewModel : BaseViewModel
    {
        private bool isPaid = true;
        private bool isNotPaid = true;
        private bool isInProgress = true;
        private bool isPending = true;
        private bool isRealized = true;
        private DateTime receiptStartDate = DateTime.Now.AddMonths(-1);
        private DateTime receiptEndDate = DateTime.Now;
        private DateTime deadlineStart = DateTime.Now.AddDays(-14);
        private DateTime deadlineEnd = DateTime.Now.AddMonths(1);

        public bool IsPaid
        {
            get => isPaid;
            set
            {
                if (!value && !IsNotPaid)
                    IsNotPaid = true;
                SetValue(ref isPaid, value);
            }
        }
        public bool IsNotPaid
        {
            get => isNotPaid;
            set
            {
                if (!value && !IsPaid)
                    IsPaid = true;
                SetValue(ref isNotPaid, value);
            }
        }
        public bool IsInProgress
        {
            get => isInProgress;
            set
            {
                if (!value && !IsPending && !IsRealized)
                {
                    DependencyService.Get<IToast>()?.MakeShortToast(ToastConstans.Min1Status);
                    IsPending = true;
                }
                SetValue(ref isInProgress, value);
            }
        }
        public bool IsPending
        {
            get => isPending;
            set
            {
                if (!value && !IsInProgress && !IsRealized)
                {
                    DependencyService.Get<IToast>()?.MakeShortToast(ToastConstans.Min1Status);
                    IsInProgress = true;
                }
                SetValue(ref isPending, value);
            }
        }
        public bool IsRealized
        {
            get => isRealized;
            set
            {
                if (!value && !IsInProgress && !IsPending)
                {
                    DependencyService.Get<IToast>()?.MakeShortToast(ToastConstans.Min1Status);
                    IsInProgress = true;
                }
                SetValue(ref isRealized, value);
            }
        }
        public DateTime ReceiptStartDate { get => receiptStartDate; set => SetValue(ref receiptStartDate, value); }
        public DateTime ReceiptEndDate { get => receiptEndDate; set => SetValue(ref receiptEndDate, value); }
        public DateTime DeadlineStart { get => deadlineStart; set => SetValue(ref deadlineStart, value); }
        public DateTime DeadlineEnd { get => deadlineEnd; set => SetValue(ref deadlineEnd, value); }

        public Command FilterOrdersCommand { get; }

        public OrderFilteringViewModel(AdditionalOrderParameters parameters)
        {
            IsInProgress = parameters.IsInProgress;
            IsNotPaid = parameters.IsNotPaid;
            IsPaid = parameters.IsPaid;
            IsPending = parameters.IsPending;
            IsRealized = parameters.IsRealized;
            DeadlineStart = parameters.DeadlineStart;
            DeadlineEnd = parameters.DeadlineEnd;
            ReceiptStartDate = parameters.ReceiptStartDate;
            ReceiptEndDate = parameters.ReceiptEndDate;

            FilterOrdersCommand = new Command(async () => await OnFilter());
        }

        private async Task OnFilter()
        {
            MessagingCenter.Send(this, MessagingCenterConstans.OrderFiltering);
            await PopupNavigation.Instance.PopAsync();
        }
    }
}