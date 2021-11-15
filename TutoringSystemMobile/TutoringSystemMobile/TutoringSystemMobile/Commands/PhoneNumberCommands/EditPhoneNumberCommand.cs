using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Models.PhoneNumberDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.PhoneNumberViewModels;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.PhoneNumberCommands
{
    public class EditPhoneNumberCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly EditPhoneNumberViewModel viewModel;
        private readonly IPhoneNumberService phoneNumberService;
        private readonly IReloadContactService reloadContactService;

        public EditPhoneNumberCommand(EditPhoneNumberViewModel viewModel, IPhoneNumberService phoneNumberService)
        {
            this.viewModel = viewModel;
            this.phoneNumberService = phoneNumberService;
            reloadContactService = DependencyService.Get<IReloadContactService>();
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(viewModel.Owner) &&
                   !string.IsNullOrWhiteSpace(viewModel.Number) &&
                   !viewModel.IsBusy;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var updated = await phoneNumberService.UpdatePhoneNumberAsync(viewModel.ContactId, new UpdatedPhoneNumberDto(viewModel.PhoneNumberId, viewModel.Owner, viewModel.Number));
            viewModel.IsBusy = false;

            if (updated)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Zaktualizowano");
                await PopupNavigation.Instance.PopAsync();
                reloadContactService.ReloadContact();
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj ponownie później!");
            }
        }
    }
}