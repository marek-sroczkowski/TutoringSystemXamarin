using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.ViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands
{
    public class LoginCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly LoginViewModel viewModel;
        private readonly IUserService userService;

        public LoginCommand(LoginViewModel viewModel, IUserService userService)
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
            return !string.IsNullOrEmpty(viewModel.Password) &&
                !string.IsNullOrEmpty(viewModel.Password);
        }

        public async void Execute(object parameter)
        {
            var loginResult = await userService.TryLoginAsync(new LoginUserDto(viewModel.Username, viewModel.Password));
            switch (loginResult)
            {
                case LoginStatus.LoggedInCorrectly:
                    await Shell.Current.GoToAsync($"//{nameof(StartTutorPage)}");
                    break;
                case LoginStatus.InactiveAccount:
                    await Shell.Current.GoToAsync($"//{nameof(AccountActivationPage)}");
                    break;
                case LoginStatus.InvalidUsernameOrPassword:
                    await Application.Current.MainPage.DisplayAlert("Uwaga!", "Nieprawidłowe dane logowania", "OK");
                    break;
            }
        }
    }
}
