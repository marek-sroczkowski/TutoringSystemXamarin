using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.ViewModels.AccountViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.AccountCommands
{
    public class LoginCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly LoginViewModel viewModel;
        private readonly IUserService userService;
        private readonly IFlyoutItemService flyoutItemService;

        public LoginCommand(LoginViewModel viewModel, IUserService userService, IFlyoutItemService flyoutItemService)
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
            return !viewModel.IsBusy &&
                !string.IsNullOrWhiteSpace(viewModel.Username) &&
                !string.IsNullOrWhiteSpace(viewModel.Password);
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var loginResult = await userService.TryLoginAsync(new LoginUserDto(viewModel.Username, viewModel.Password));
            viewModel.IsBusy = false;

            switch (loginResult)
            {
                case LoginStatus.LoggedInCorrectly:
                    await LoggedInCorrectly();
                    break;
                case LoginStatus.InactiveAccount:
                    await InactiveAccount();
                    break;
                case LoginStatus.InvalidUsernameOrPassword:
                    await InvalidUsernameOrPassword();
                    break;
            }
        }

        private async Task LoggedInCorrectly()
        {
            viewModel.IsBusy = true;
            var role = await userService.GetUserRole();
            viewModel.IsBusy = false;

            switch (role)
            {
                case Role.Tutor:
                    await LoggedAsTutor();
                    break;
                case Role.Student:
                    await LoggedAdStudent();
                    break;
            }
        }

        private async Task InactiveAccount()
        {
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.InactiveAccount.ToString());
            await Shell.Current.GoToAsync($"//{nameof(AccountActivationPage)}");
        }

        private async Task InvalidUsernameOrPassword()
        {
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedOut.ToString());
            await Application.Current.MainPage.DisplayAlert("Uwaga!", "Nieprawidłowe dane logowania", "OK");
        }

        private async Task LoggedAsTutor()
        {
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedAsTutor.ToString());
            flyoutItemService.EnableTutorFlyoutItems();
            await Shell.Current.GoToAsync($"//{nameof(StartTutorPage)}");
        }

        private async Task LoggedAdStudent()
        {
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedAsStudent.ToString());
            flyoutItemService.EnableStudentFlyoutItems();
            await Shell.Current.GoToAsync($"//{nameof(StartStudentPage)}");
        }
    }
}