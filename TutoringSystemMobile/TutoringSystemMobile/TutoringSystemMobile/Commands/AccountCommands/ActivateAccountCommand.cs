using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.ViewModels.AccountViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.AccountCommands
{
    public class ActivateAccountCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private AccountActivationViewModel viewModel;
        private readonly IUserService userService;

        public ActivateAccountCommand(AccountActivationViewModel viewModel, IUserService userService)
        {
            this.viewModel = viewModel;
            this.userService = userService;
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(viewModel.ActivationToken) &&
                viewModel.ActivationToken.Length == 6;
        }

        public async void Execute(object parameter)
        {
            if (await userService.ActivateUserByTokenAsync(viewModel.ActivationToken))
                await Shell.Current.GoToAsync($"//{nameof(StartTutorPage)}");
            else
                await Application.Current.MainPage.DisplayAlert("Uwaga!", "Nieprawidłowy kod", "OK");
        }
    }
}
