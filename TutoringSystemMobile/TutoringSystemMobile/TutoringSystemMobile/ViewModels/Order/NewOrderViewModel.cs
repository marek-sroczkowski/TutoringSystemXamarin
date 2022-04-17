using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.AdditionalOrder;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Order
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

        public Command AddNewOrderCommand { get; }

        private readonly IAdditionalOrderService orderService = DependencyService.Get<IAdditionalOrderService>();

        public NewOrderViewModel()
        {
            AddNewOrderCommand = new Command(async () => await OnAddNewOrder(), CanAddNewOrder);
            PropertyChanged += (_, __) => AddNewOrderCommand.ChangeCanExecute();
        }

        public bool CanAddNewOrder()
        {
            return !Name.IsEmpty()
                && !Cost.IsEmpty()
                && double.TryParse(Cost, out double cost)
                && cost > 0
                && Deadline.HasValue
                && !IsBusy;
        }

        private async Task OnAddNewOrder()
        {
            IsBusy = true;
            var newOrder = new NewOrderDto(Name, Deadline.Value, Description, Cost.ToDouble(), IsPaid);
            long newOrderId = await orderService.AddOrderAsync(newOrder);
            IsBusy = false;

            if (newOrderId == -1)
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.AddedOrder);
                await Shell.Current.GoToAsync($"//{nameof(OrdersTutorPage)}/{nameof(OrderDetailsTutorPage)}?{nameof(OrderDetailsViewModel.Id)}={newOrderId}");
            }
        }
    }
}