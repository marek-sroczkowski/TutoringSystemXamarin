using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Dtos.Account;
using TutoringSystemMobile.Models.Errors;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Account
{
    public class RegisterStudentViewModel : BaseViewModel
    {
        private string email;
        private string password = "";
        private string confirmPassword = "";
        private bool isPasswordIncorrect;
        private bool isConfirmPasswordIncorrect;
        private bool isEmailIncorrect;

        public string Email { get => email; set => SetValue(ref email, value); }
        public string Password { get => password; set => SetValue(ref password, value); }
        public string ConfirmPassword { get => confirmPassword; set => SetValue(ref confirmPassword, value); }

        public bool IsPasswordIncorrect { get => isPasswordIncorrect; set => SetValue(ref isPasswordIncorrect, value); }
        public bool IsConfirmPasswordIncorrect { get => isConfirmPasswordIncorrect; set => SetValue(ref isConfirmPasswordIncorrect, value); }
        public bool IsEmailIncorrect { get => isEmailIncorrect; set => SetValue(ref isEmailIncorrect, value); }

        public Command RegisterCommand { get; }

        public RegisterStudentViewModel()
        {
            RegisterCommand = new Command(async () => await OnRegister(), CanRegister);
            PropertyChanged += (_, __) => RegisterCommand.ChangeCanExecute();
        }

        public bool CanRegister()
        {
            IsPasswordIncorrect = !Password.IsEmpty() && !Regex.IsMatch(Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            IsConfirmPasswordIncorrect = !ConfirmPassword.IsEmpty() && !Password.Equals(ConfirmPassword);
            IsEmailIncorrect = !Email.IsEmpty() && !IsValidEmail(Email);

            return !Email.IsEmpty()
                && Regex.IsMatch(Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$")
                && IsValidEmail(Email)
                && Password.Equals(ConfirmPassword)
                && !IsBusy;
        }

        private async Task OnRegister()
        {
            IsBusy = true;
            var errors = await DependencyService.Get<IUserService>()
                .RegisterStudentAsync(new RegisteredStudentDto(Email, Password, ConfirmPassword));
            IsBusy = false;

            if (errors is null)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.SuccessfulRegistration);
                await Shell.Current.GoToAsync($"//{nameof(AccountActivationPage)}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, GetErrorsMessage(errors), GeneralConstans.Ok);
            }
        }

        public string GetErrorsMessage(RegisterErrors errors)
        {
            StringBuilder builder = new StringBuilder($"{ToastConstans.RegistrationFailed}\n");

            if (errors.Email != null)
            {
                builder.AppendLine(ToastConstans.TakenEmail);
            }
            if (errors.Username != null)
            {
                builder.AppendLine(ToastConstans.TakenLogin);
            }
            if (errors.Password != null)
            {
                builder.AppendLine(ToastConstans.IncorrectPassword);
            }

            return builder.ToString();
        }

        bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }

            try
            {
                return new MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}