using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.ViewModels.AccountViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.AccountCommands
{
    public class ActivateAccountCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly AccountActivationViewModel viewModel;
        private readonly IUserService userService;
        private readonly IFlyoutItemService flyoutItemService;

        public ActivateAccountCommand(AccountActivationViewModel viewModel, IUserService userService, IFlyoutItemService flyoutItemService)
        {
            this.viewModel = viewModel;
            this.userService = userService;
            this.flyoutItemService = flyoutItemService;
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(viewModel.ActivationToken) &&
                viewModel.ActivationToken.Length == 6 &&
                !viewModel.IsBusy;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var activated = await userService.ActivateUserByTokenAsync(viewModel.ActivationToken);
            viewModel.IsBusy = false;

            if (activated)
            {
                flyoutItemService.EnableTutorFlyoutItems();
                await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedAsTutor.ToString());
                await Shell.Current.GoToAsync($"//{nameof(StartTutorPage)}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Uwaga!", "Nieprawidłowy kod", "OK");
            }
        }
    }
}
