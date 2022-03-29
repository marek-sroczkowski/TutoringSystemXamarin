using Plugin.FirebasePushNotification;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Dtos.Account;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Dtos.PushNotificationToken;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;

namespace TutoringSystemMobile.ViewModels.Account
{
    public class LoginViewModel : BaseViewModel
    {
        private string username = "";
        private string password = "";

        public string Username { get => username; set => SetValue(ref username, value); }
        public string Password { get => password; set => SetValue(ref password, value); }

        public Command LoginCommand { get; }
        public Command RegisterTutorFormCommand { get; }

        private readonly IUserService userService = DependencyService.Get<IUserService>();
        private readonly IFlyoutService flyoutService = DependencyService.Get<IFlyoutService>();

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await OnLogin(), CanLogin);
            PropertyChanged += (_, __) => LoginCommand.ChangeCanExecute();

            RegisterTutorFormCommand = new Command(async () => await OnRegisterFormClick());
        }

        public async Task OnRegisterFormClick()
        {
            if (!IsBusy)
            {
                await Shell.Current.GoToAsync($"{nameof(RegisterTutorPage)}");
            }
        }

        public bool CanLogin()
        {
            return !Username.IsEmpty()
                && !IsBusy;
        }

        private async Task OnLogin()
        {
            IsBusy = true;
            var loginUser = new LoginUserDto(Username, Password);
            var loginResult = await userService.TryLoginAsync(loginUser);
            IsBusy = false;

            switch (loginResult.LoginStatus)
            {
                case LoginStatus.LoggedInCorrectly:
                    await LoggedInCorrectly(loginResult.User);
                    break;
                case LoginStatus.InactiveAccount:
                    await InactiveAccount();
                    break;
                case LoginStatus.UnregisteredStudent:
                    await UnregisteredStudent();
                    break;
                case LoginStatus.InvalidUsernameOrPassword:
                    await InvalidUsernameOrPassword();
                    break;
            }
        }

        private async Task LoggedInCorrectly(UserDto user)
        {
            await SecureStorage.SetAsync(SecureStorageConstans.UserName, $"{user.FirstName} {user.LastName}");
            //await DependencyService.Get<IPushNotificationTokenService>().PutTokenAsync();

            switch (user.Role)
            {
                case Role.Tutor:
                    await LoggedAsTutor();
                    break;
                case Role.Student:
                    await LoggedAsStudent();
                    break;
            }
        }

        private async Task InactiveAccount()
        {
            Settings.LoginStatus = AccountStatus.InactiveAccount;
            await Shell.Current.GoToAsync($"//{nameof(AccountActivationPage)}");
        }

        private async Task UnregisteredStudent()
        {
            Settings.LoginStatus = AccountStatus.InactiveAccount;
            await Shell.Current.GoToAsync($"{nameof(RegisterStudentPage)}");
        }

        private async Task InvalidUsernameOrPassword()
        {
            Settings.LoginStatus = AccountStatus.LoggedOut;
            await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.InvalidLogin, GeneralConstans.Ok);
        }

        private async Task LoggedAsTutor()
        {
            Settings.LoginStatus = AccountStatus.LoggedAsTutor;
            flyoutService.EnableTutorFlyoutItems();
            await Shell.Current.GoToAsync($"//{nameof(StartTutorPage)}");
        }

        private async Task LoggedAsStudent()
        {
            Settings.LoginStatus = AccountStatus.LoggedAsStudent;
            flyoutService.EnableStudentFlyoutItems();
            await Shell.Current.GoToAsync($"//{nameof(StartStudentPage)}");
        }
    }
}