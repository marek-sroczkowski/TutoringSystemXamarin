﻿using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Account
{
    public class AccountActivationViewModel : BaseViewModel
    {
        private string activationToken;

        public string ActivationToken { get => activationToken; set => SetValue(ref activationToken, value); }

        public Command ActivateAccountCommand { get; }
        public Command NewActivationCodeCommand { get; }

        private readonly IUserService userService;

        public AccountActivationViewModel()
        {
            userService = DependencyService.Get<IUserService>();

            ActivateAccountCommand = new Command(async () => await OnActivateAccount(), CanActivateAccount);
            PropertyChanged += (_, __) => ActivateAccountCommand.ChangeCanExecute();
            NewActivationCodeCommand = new Command(async () => await OnNewActivationCode(), CanNewActivationCode);
            PropertyChanged += (_, __) => NewActivationCodeCommand.ChangeCanExecute();
        }

        public bool CanActivateAccount()
        {
            return !ActivationToken.IsEmpty()
                && ActivationToken.Length == 6
                && !IsBusy;
        }

        private async Task OnActivateAccount()
        {
            IsBusy = true;
            var activated = await userService.ActivateUserByTokenAsync(ActivationToken);
            IsBusy = false;

            if (activated)
            {
                await RedirectToStartPage();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.IncorrectActivationCode, GeneralConstans.Ok);
            }
        }

        public bool CanNewActivationCode()
        {
            return !IsBusy;
        }

        private async Task OnNewActivationCode()
        {
            IsBusy = true;
            var sent = await userService.SendNewActivationTokenAsync();
            IsBusy = false;

            if (sent)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.SentActivationCode);
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorActivationCodeSending);
            }
        }

        private async Task RedirectToStartPage()
        {
            var role = await userService.GetUserRole();

            if (role == Role.Tutor)
            {
                await LogInAsTutor();
            }
            else
            {
                await LogInAsStudent();
            }
        }

        private async Task LogInAsTutor()
        {
            DependencyService.Get<IFlyoutItemService>().EnableTutorFlyoutItems();
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedAsTutor.ToString());
            await Shell.Current.GoToAsync($"//{nameof(StartTutorPage)}");
        }

        private async Task LogInAsStudent()
        {
            DependencyService.Get<IFlyoutItemService>().EnableStudentFlyoutItems();
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedAsStudent.ToString());
            await Shell.Current.GoToAsync($"//{nameof(StartStudentPage)}");
        }
    }
}