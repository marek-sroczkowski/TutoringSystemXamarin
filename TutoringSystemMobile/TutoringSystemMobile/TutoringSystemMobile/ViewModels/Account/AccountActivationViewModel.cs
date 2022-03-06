using System.Threading.Tasks;
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

        public AccountActivationViewModel()
        {
            ActivateAccountCommand = new Command(async () => await OnActivateAccount(), CanActivateAccount);
            PropertyChanged += (_, __) => ActivateAccountCommand.ChangeCanExecute();
            NewActivationCodeCommand = new Command(async () => await OnNewActivationCode(), CanNewActivationCode);
            PropertyChanged += (_, __) => NewActivationCodeCommand.ChangeCanExecute();
        }

        public bool CanActivateAccount()
        {
            return !ActivationToken.IsEmpty() &&
                ActivationToken.Length == 6 &&
                !IsBusy;
        }

        private async Task OnActivateAccount()
        {
            IsBusy = true;
            var activated = await DependencyService.Get<IUserService>()
                .ActivateUserByTokenAsync(ActivationToken);
            IsBusy = false;

            if (activated)
            {
                DependencyService.Get<IFlyoutItemService>().EnableTutorFlyoutItems();
                await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedAsTutor.ToString());
                await Shell.Current.GoToAsync($"//{nameof(StartTutorPage)}");
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
            var sent = await DependencyService.Get<IUserService>().SendNewActivationTokenAsync();
            IsBusy = false;

            if (sent)
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.SentActivationCode);
            else
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorActivationCodeSending);
        }
    }
}