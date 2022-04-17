using Rg.Plugins.Popup.Services;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Account;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Profile
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private string oldPassword;
        private string newPassword = "";
        private string confirmPassword = "";
        private bool isNewPasswordIncorrect;
        private bool isConfirmPasswordIncorrect;

        public string OldPassword { get => oldPassword; set => SetValue(ref oldPassword, value); }
        public string NewPassword { get => newPassword; set => SetValue(ref newPassword, value); }
        public string ConfirmPassword { get => confirmPassword; set => SetValue(ref confirmPassword, value); }
        public bool IsNewPasswordIncorrect { get => isNewPasswordIncorrect; set => SetValue(ref isNewPasswordIncorrect, value); }
        public bool IsConfirmPasswordIncorrect { get => isConfirmPasswordIncorrect; set => SetValue(ref isConfirmPasswordIncorrect, value); }

        public Command ChangePasswordCommand { get; set; }

        private readonly IUserService userService = DependencyService.Get<IUserService>();

        public ChangePasswordViewModel()
        {
            ChangePasswordCommand = new Command(async () => await OnChangePassword(), CanChangePassword);
            PropertyChanged += (_, __) => ChangePasswordCommand.ChangeCanExecute();
        }

        public bool CanChangePassword()
        {
            IsNewPasswordIncorrect = !NewPassword.IsEmpty() && !Regex.IsMatch(NewPassword, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            IsConfirmPasswordIncorrect = !ConfirmPassword.IsEmpty() && !NewPassword.Equals(ConfirmPassword);

            return !OldPassword.IsEmpty()
                && Regex.IsMatch(NewPassword, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$")
                && NewPassword.Equals(ConfirmPassword)
                && !IsBusy;
        }

        private async Task OnChangePassword()
        {
            IsBusy = true;
            var password = new PasswordDto(NewPassword, ConfirmPassword, OldPassword);
            var errors = await userService.ChangePasswordAsync(password);
            IsBusy = false;

            if (errors is null)
            {
                ToastHelper.MakeLongToast(ToastConstans.ChangedPassword);
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, errors.ToString(), GeneralConstans.Ok);
            }
        }
    }
}