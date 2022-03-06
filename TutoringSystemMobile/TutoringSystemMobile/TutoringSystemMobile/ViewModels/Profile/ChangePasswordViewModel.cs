using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Dtos.Account;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Profile
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private string oldPassword;
        private string newPassword = "";
        private string confirmPassword = "";
        private bool isNewPasswordIncorrect = false;
        private bool isConfirmPasswordIncorrect = false;

        public string OldPassword { get => oldPassword; set => SetValue(ref oldPassword, value); }
        public string NewPassword { get => newPassword; set => SetValue(ref newPassword, value); }
        public string ConfirmPassword { get => confirmPassword; set => SetValue(ref confirmPassword, value); }
        public bool IsNewPasswordIncorrect { get => isNewPasswordIncorrect; set => SetValue(ref isNewPasswordIncorrect, value); }
        public bool IsConfirmPasswordIncorrect { get => isConfirmPasswordIncorrect; set => SetValue(ref isConfirmPasswordIncorrect, value); }

        public Command ChangePasswordCommand { get; set; }

        public ChangePasswordViewModel()
        {
            ChangePasswordCommand = new Command(async () => await OnChangePassword(), CanChangePassword);
            PropertyChanged += (_, __) => ChangePasswordCommand.ChangeCanExecute();
        }

        public bool CanChangePassword()
        {
            IsNewPasswordIncorrect = !NewPassword.IsEmpty()
                && !Regex.IsMatch(NewPassword, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            IsConfirmPasswordIncorrect = !ConfirmPassword.IsEmpty() &&
                !NewPassword.Equals(ConfirmPassword);

            return !OldPassword.IsEmpty() &&
                Regex.IsMatch(NewPassword, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$") &&
                NewPassword.Equals(ConfirmPassword) &&
                !IsBusy;
        }

        private async Task OnChangePassword()
        {
            IsBusy = true;
            var errors = await DependencyService.Get<IUserService>()
                .ChangePasswordAsync(new PasswordDto(NewPassword, ConfirmPassword, OldPassword));
            IsBusy = false;

            if (errors is null)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ChangedPassword);
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, GetErrorsMessage(errors), GeneralConstans.Ok);
            }
        }

        private string GetErrorsMessage(IEnumerable<WrongPasswordStatus> errors)
        {
            StringBuilder builder = new StringBuilder($"{ToastConstans.PasswordChangeFailed}\n");
            foreach (var error in errors)
            {
                switch (error)
                {
                    case WrongPasswordStatus.PasswordsVary:
                        builder.AppendLine(ToastConstans.PasswordsVary);
                        break;
                    case WrongPasswordStatus.TooShort:
                        builder.AppendLine(ToastConstans.TooShortPassword);
                        break;
                    case WrongPasswordStatus.DuplicateOfOld:
                        builder.AppendLine(ToastConstans.DuplicateOfOldPassword);
                        break;
                    case WrongPasswordStatus.InvalidOldPassword:
                        builder.AppendLine(ToastConstans.InvalidOldPassword);
                        break;
                    case WrongPasswordStatus.InternalError:
                    default:
                        builder.AppendLine(ToastConstans.InternalError);
                        break;
                }
            }

            return builder.ToString();
        }
    }
}