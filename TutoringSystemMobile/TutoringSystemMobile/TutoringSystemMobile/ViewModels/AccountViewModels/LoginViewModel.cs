﻿using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.AccountViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string username = "";
        private string password = "";

        public string Username { get => username; set => SetValue(ref username, value); }
        public string Password { get => password; set => SetValue(ref password, value); }

        public Command LoginCommand { get; }
        public Command RegisterTutorFormCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await OnLogin(), CanLogin);
            PropertyChanged += (_, __) => LoginCommand.ChangeCanExecute();
            RegisterTutorFormCommand = new Command(async () => await OnRegisterFormClick());
        }

        public async Task OnRegisterFormClick()
        {
            if (!IsBusy)
                await Shell.Current.GoToAsync($"{nameof(RegisterTutorPage)}");
        }

        public bool CanLogin()
        {
            return !Username.IsEmpty() &&
                !Password.IsEmpty() &&
                !IsBusy;
        }

        private async Task OnLogin()
        {
            IsBusy = true;
            var loginResult = await DependencyService.Get<IUserService>()
                .TryLoginAsync(new LoginUserDto(Username, Password));
            IsBusy = false;

            switch (loginResult.LoginStatus)
            {
                case LoginStatus.LoggedInCorrectly:
                    await LoggedInCorrectly(loginResult.User);
                    break;
                case LoginStatus.InactiveAccount:
                    await InactiveAccount();
                    break;
                case LoginStatus.InvalidUsernameOrPassword:
                    await InvalidUsernameOrPassword();
                    break;
            }
        }

        private async Task LoggedInCorrectly(UserDto user)
        {
            await SecureStorage.SetAsync(SecureStorageConstans.UserName, $"{user.FirstName} {user.LastName}");

            switch (user.Role)
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
            await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.InvalidLogin, GeneralConstans.Ok);
        }

        private async Task LoggedAsTutor()
        {
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedAsTutor.ToString());
            DependencyService.Get<IFlyoutItemService>().EnableTutorFlyoutItems();
            await Shell.Current.GoToAsync($"//{nameof(StartTutorPage)}");
        }

        private async Task LoggedAdStudent()
        {
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedAsStudent.ToString());
            DependencyService.Get<IFlyoutItemService>().EnableStudentFlyoutItems();
            await Shell.Current.GoToAsync($"//{nameof(StartStudentPage)}");
        }
    }
}