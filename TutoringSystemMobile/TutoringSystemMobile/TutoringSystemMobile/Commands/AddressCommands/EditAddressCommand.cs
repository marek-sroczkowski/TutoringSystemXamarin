using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Models.AddressDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.AddressViewModels;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.AddressCommands
{
    public class EditAddressCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly EditAddressViewModel viewModel;
        private readonly IAddressService addressService;

        public EditAddressCommand(EditAddressViewModel viewModel, IAddressService addressService)
        {
            this.viewModel = viewModel;
            this.addressService = addressService;
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

            var updated = await addressService.UpdateAddressAsync(new UpdatedAddressDto(viewModel.Id, viewModel.Street, viewModel.HouseAndFlatNumber, viewModel.City, viewModel.PostalCode, viewModel.Description));
            if (updated)
                await Shell.Current.GoToAsync("..");
            else
                DependencyService.Get<IToast>()?.MakeToast("Błąd! Spróbuj ponownie później!");

            viewModel.IsBusy = false;
        }
    }
}