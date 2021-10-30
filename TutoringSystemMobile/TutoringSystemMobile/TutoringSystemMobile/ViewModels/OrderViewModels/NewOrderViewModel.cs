using System;
using System.Windows.Input;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.ViewModels.OrderViewModels
{
    public class NewOrderViewModel : BaseViewModel
    {
        private long id;
        private string name;
        private DateTime deadline;
        private double cost;
        private bool isPaid;
        private AdditionalOrderStatus status;

        public long Id { get => id; set => SetValue(ref id, value); }
        public string Name { get => name; set => SetValue(ref name, value); }
        public DateTime Deadline { get => deadline; set => SetValue(ref deadline, value); }
        public double Cost { get => cost; set => SetValue(ref cost, value); }
        public bool IsPaid { get => isPaid; set => SetValue(ref isPaid, value); }
        public AdditionalOrderStatus Status { get => status; set => SetValue(ref status, value); }

        public ICommand NewOrderFormCommand { get; }
        public ICommand OrderDetailsCommand { get; }

        public NewOrderViewModel()
        {

        }
    }
}
