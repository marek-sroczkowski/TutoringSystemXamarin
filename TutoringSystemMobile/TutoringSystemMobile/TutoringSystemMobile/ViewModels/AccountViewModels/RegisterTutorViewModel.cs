using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Models.Errors;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.AccountViewModels
{
    public class RegisterTutorViewModel : BaseViewModel
    {
        private string username;
        private string firstName;
        private string email;
        private string password = "";
        private string confirmPassword = "";
        private bool isPasswordIncorrect = false;
        private bool isConfirmPasswordIncorrect = false;
        private bool isEmailIncorrect;

        public string Username { get => username; set => SetValue(ref username, value); }
        public string FirstName { get => firstName; set => SetValue(ref firstName, value); }
        public string Email { get => email; set => SetValue(ref email, value); }
        public string Password { get => password; set => SetValue(ref password, value); }
        public string ConfirmPassword { get => confirmPassword; set => SetValue(ref confirmPassword, value); }

        public bool IsPasswordIncorrect { get => isPasswordIncorrect; set => SetValue(ref isPasswordIncorrect, value); }
        public bool IsConfirmPasswordIncorrect { get => isConfirmPasswordIncorrect; set => SetValue(ref isConfirmPasswordIncorrect, value); }
        public bool IsEmailIncorrect { get => isEmailIncorrect; set => SetValue(ref isEmailIncorrect, value); }

        public Command RegisterCommand { get; }

        public RegisterTutorViewModel()
        {
            RegisterCommand = new Command(async () => await OnRegister(), CanRegister);
            PropertyChanged += (_, __) => RegisterCommand.ChangeCanExecute();
        }

        public bool CanRegister()
        {
            IsPasswordIncorrect = !Password.IsEmpty()
                && !Regex.IsMatch(Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            IsConfirmPasswordIncorrect = !ConfirmPassword.IsEmpty() &&
                !Password.Equals(ConfirmPassword);
            IsEmailIncorrect = !Email.IsEmpty() &&
                !IsValidEmail(Email);

            return !Username.IsEmpty() &&
                !FirstName.IsEmpty() &&
                !Email.IsEmpty() &&
                Regex.IsMatch(Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$") &&
                IsValidEmail(Email) &&
                Password.Equals(ConfirmPassword) &&
                !IsBusy;
        }

        private async Task OnRegister()
        {
            IsBusy = true;
            var errors = await DependencyService.Get<IUserService>()
                .RegisterTutorAsync(new RegisterTutorDto(Username, FirstName, Email, Password, ConfirmPassword));
            IsBusy = false;

            if (errors is null)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.SuccessfulRegistration);
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, GetErrorsMessage(errors), GeneralConstans.Ok);
            }
        }

        public string GetErrorsMessage(RegisterErrorTypes errors)
        {
            StringBuilder builder = new StringBuilder($"{ToastConstans.RegistrationFailed}\n");
            if (errors.Email != null)
                builder.AppendLine(ToastConstans.TakenEmail);
            if (errors.Username != null)
                builder.AppendLine(ToastConstans.TakenLogin);
            if (errors.Password != null)
                builder.AppendLine(ToastConstans.IncorrectPassword);

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
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}