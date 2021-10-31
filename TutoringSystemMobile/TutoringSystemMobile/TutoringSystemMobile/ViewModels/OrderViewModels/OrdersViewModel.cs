using System.Collections.ObjectModel;
using System.Windows.Input;
using TutoringSystemMobile.Commands.OrderCommands;
using TutoringSystemMobile.Models.AdditionalOrderDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.OrderViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        private OrderDto selectedOrder;
        public ObservableCollection<OrderDto> Orders { get; }

        public OrderDto SelectedOrder
        {
            get => selectedOrder;
            set
            {
                SetValue(ref selectedOrder, value);
                OnOrderSelected(value);
            }
        }

        public ICommand LoadOrdersCommand { get; }
        public Command NewOrderFormCommand { get; }
        public Command FilterOrdersCommand { get; }
        public Command<OrderDto> OrderTapped { get; }
        public Command PageAppearingCommand { get; }

        public OrdersViewModel()
        {
            Orders = new ObservableCollection<OrderDto>();
            LoadOrdersCommand = new LoadOrdersCommand(this, DependencyService.Get<IAdditionalOrderService>());
            NewOrderFormCommand = new Command(OnNewOrderClick);
            FilterOrdersCommand = new Command(OnFilteringOrdersClick);
            OrderTapped = new Command<OrderDto>(OnOrderSelected);
            PageAppearingCommand = new Command(OnAppearing);
        }

        private void OnAppearing()
        {
            IsBusy = true;
            selectedOrder = null;
        }

        async void OnOrderSelected(OrderDto order)
        {
            if (order == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}?{nameof(OrderDetailsViewModel.Id)}={order.Id}");
        }

        private async void OnNewOrderClick()
        {
            await Shell.Current.GoToAsync($"{nameof(NewOrderPage)}");
        }

        private async void OnFilteringOrdersClick()
        {
            await Shell.Current.GoToAsync($"{nameof(OrderFilteringPage)}");
        }
    }
}
