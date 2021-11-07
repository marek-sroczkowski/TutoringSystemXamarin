using System;
using System.Windows.Input;
using TutoringSystemMobile.Commands.OrderCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.OrderViewModels
{
    public class NewOrderViewModel : BaseViewModel
    {
        private string name;
        private DateTime? deadline = DateTime.Now;
        private string cost;
        private bool isPaid;
        private string description;

        public string Name { get => name; set => SetValue(ref name, value); }
        public DateTime? Deadline { get => deadline; set => SetValue(ref deadline, value); }
        public string Cost { get => cost; set => SetValue(ref cost, value); }
        public bool IsPaid { get => isPaid; set => SetValue(ref isPaid, value); }
        public string Description { get => description; set => SetValue(ref description, value); }

        public ICommand AddNewOrderCommand { get; }

        public NewOrderViewModel()
        {
            AddNewOrderCommand = new AddNewOrderCommand(this, DependencyService.Get<IAdditionalOrderService>());
        }
    }
}
