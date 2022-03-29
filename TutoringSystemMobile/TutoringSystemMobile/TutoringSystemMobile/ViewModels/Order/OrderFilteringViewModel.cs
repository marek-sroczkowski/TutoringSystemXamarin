using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Parameters;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Order
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

        public bool IsPaid { get => isPaid; set => SetIsPaid(value); }
        public bool IsNotPaid { get => isNotPaid; set => SetIsNotPaid(value); }
        public bool IsInProgress { get => isInProgress; set => SetIsInProgress(value); }
        public bool IsPending { get => isPending; set => SetIsPending(value); }
        public bool IsRealized { get => isRealized; set => SetIsRealized(value); }

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

        private void SetIsPaid(bool value)
        {
            if (!value && !IsNotPaid)
            {
                IsNotPaid = true;
            }

            SetValue(ref isPaid, value);
        }

        private void SetIsNotPaid(bool value)
        {
            if (!value && !IsPaid)
            {
                IsPaid = true;
            }

            SetValue(ref isNotPaid, value);
        }

        private void SetIsInProgress(bool value)
        {
            if (!value && !IsPending && !IsRealized)
            {
                ToastHelper.MakeShortToast(ToastConstans.Min1Status);
                IsPending = true;
            }

            SetValue(ref isInProgress, value);
        }

        private void SetIsPending(bool value)
        {
            if (!value && !IsInProgress && !IsRealized)
            {
                ToastHelper.MakeShortToast(ToastConstans.Min1Status);
                IsInProgress = true;
            }

            SetValue(ref isPending, value);
        }

        private void SetIsRealized(bool value)
        {
            if (!value && !IsInProgress && !IsPending)
            {
                ToastHelper.MakeShortToast(ToastConstans.Min1Status);
                IsInProgress = true;
            }

            SetValue(ref isRealized, value);
        }
    }
}