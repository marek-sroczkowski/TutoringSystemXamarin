using System.Windows.Input;
using TutoringSystemMobile.Commands.ProfileCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.ProfileViewModels
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

        public ICommand ChangePasswordCommand { get; set; }

        public ChangePasswordViewModel()
        {
            ChangePasswordCommand = new ChangePasswordCommand(this, DependencyService.Get<IUserService>());
        }
    }
}