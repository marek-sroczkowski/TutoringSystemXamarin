using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Account;
using TutoringSystemMobile.Services.Interfaces;
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

        private readonly IUserService userService = DependencyService.Get<IUserService>();

        public RegisterStudentViewModel()
        {
            RegisterCommand = new Command(async () => await OnRegister(), CanRegister);
            PropertyChanged += (_, __) => RegisterCommand.ChangeCanExecute();
        }

        public bool CanRegister()
        {
            IsPasswordIncorrect = !Password.IsEmpty() && !Regex.IsMatch(Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            IsConfirmPasswordIncorrect = !ConfirmPassword.IsEmpty() && !Password.Equals(ConfirmPassword);
            IsEmailIncorrect = !Email.IsEmpty() && !Email.IsValidEmail();

            return !Email.IsEmpty()
                && Regex.IsMatch(Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$")
                && Email.IsValidEmail()
                && Password.Equals(ConfirmPassword)
                && !IsBusy;
        }

        private async Task OnRegister()
        {
            IsBusy = true;
            var registeredStudent = new RegisteredStudentDto(Email, Password, ConfirmPassword);
            var errors = await userService.RegisterStudentAsync(registeredStudent);
            IsBusy = false;

            if (errors is null)
            {
                ToastHelper.MakeLongToast(ToastConstans.SuccessfulRegistration);
                await Shell.Current.GoToAsync($"//{nameof(AccountActivationPage)}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, errors.ToString(), GeneralConstans.Ok);
            }
        }
    }
}