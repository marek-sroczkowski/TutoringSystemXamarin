using System.Windows.Input;
using TutoringSystemMobile.Commands.AccountCommands;
using TutoringSystemMobile.Services.Interfaces;
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

        public ICommand RegisterCommand { get; }

        public RegisterTutorViewModel()
        {
            RegisterCommand = new RegisterTutorCommand(this, DependencyService.Get<IUserService>());
        }
    }
}