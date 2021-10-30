using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.ViewModels.OrderViewModels;

namespace TutoringSystemMobile.Commands.OrderCommands
{
    public class LoadOrdersCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly OrdersViewModel viewModel;
        private readonly IAdditionalOrderService orderService;

        public LoadOrdersCommand(OrdersViewModel viewModel, IAdditionalOrderService orderService)
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
            return true;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            viewModel.Orders.Clear();
            var orders = await orderService.GetAdditionalOrdersAsync(new Models.Parameters.AdditionalOrderParameters());
            foreach (var order in orders)
                viewModel.Orders.Add(order);

            viewModel.IsBusy = false;
        }
    }
}
