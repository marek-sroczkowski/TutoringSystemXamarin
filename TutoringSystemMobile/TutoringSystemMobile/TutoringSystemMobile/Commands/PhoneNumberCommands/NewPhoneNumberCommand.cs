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
    public class NewPhoneNumberCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly NewPhoneNumberViewModel viewModel;
        private readonly IPhoneNumberService phoneNumberService;
        private readonly IReloadContactService reloadContactService;

        public NewPhoneNumberCommand(NewPhoneNumberViewModel viewModel, IPhoneNumberService phoneNumberService)
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
            return !string.IsNullOrEmpty(viewModel.Owner) &&
                !string.IsNullOrEmpty(viewModel.Number);
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var added = await phoneNumberService.AddPhoneNumberAsync(viewModel.ContactId, new NewPhoneNumberDto(viewModel.Owner, viewModel.Number));
            viewModel.IsBusy = false;

            if (added)
            {
                await PopupNavigation.Instance.PopAsync();
                reloadContactService.ReloadContact();
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeToast("Błąd! Spróbuj ponownie później!");
            }
        }
    }
}