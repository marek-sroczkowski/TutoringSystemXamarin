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
    public class EditOrderCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly EditOrderViewModel viewModel;
        private readonly IAdditionalOrderService orderService;

        public EditOrderCommand(EditOrderViewModel viewModel, IAdditionalOrderService orderService)
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
            //return !string.IsNullOrEmpty(viewModel.Name) &&
            //    viewModel.Deadline.HasValue &&
            //    viewModel.Cost.HasValue &&
            //    !viewModel.IsBusy;
            return true;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            bool updated = await orderService.UpdateAdditionalOrderAsync(new UpdatedOrderDto(viewModel.Id, viewModel.Name, viewModel.Deadline,viewModel.Description, viewModel.Cost, viewModel.IsPaid, viewModel.Status));
            viewModel.IsBusy = false;

            if (updated)
            {
                await Shell.Current.GoToAsync($"//{nameof(OrdersTutorPage)}/{nameof(OrderDetailsPage)}?{nameof(OrderDetailsViewModel.Id)}={viewModel.Id}");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeToast("Błąd! Spróbuj ponownie");
            }
        }
    }
}
