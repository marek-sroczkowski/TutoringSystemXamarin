using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Models.AdditionalOrderDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.OrderViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.OrderCommands
{
    public class AddNewOrderCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly NewOrderViewModel viewModel;
        private readonly IAdditionalOrderService orderService;

        public AddNewOrderCommand(NewOrderViewModel viewModel, IAdditionalOrderService orderService)
        {
            this.viewModel = viewModel;
            this.orderService = orderService;
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(viewModel.Name) &&
                viewModel.Deadline.HasValue &&
                viewModel.Cost.HasValue &&
                !viewModel.IsBusy;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            long newOrderId = await orderService.AddAdditionalOrderAsync(new NewOrderDto(viewModel.Name, viewModel.Deadline.Value, viewModel.Description, viewModel.Cost.Value, viewModel.IsPaid));
            viewModel.IsBusy = false;

            if (newOrderId == -1)
            {
                DependencyService.Get<IToast>()?.MakeToast("Błąd! Spróbuj ponownie później");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeToast("Dodano zlecenie");
                await Shell.Current.GoToAsync($"//{nameof(OrdersTutorPage)}/{nameof(OrderDetailsTutorPage)}?{nameof(OrderDetailsViewModel.Id)}={newOrderId}");
            }
        }
    }
}
