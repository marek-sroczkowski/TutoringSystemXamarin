using System.Windows.Input;
using TutoringSystemMobile.Commands.StudentCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.StudentViewModels
{
    public class RegisterStudentViewModel : BaseViewModel
    {
        private string username;
        private string firstName;
        private string password = "";
        private string confirmPassword = "";
        private string hourRate;
        private string lastName;
        private string note;
        private bool isPasswordIncorrect = false;
        private bool isConfirmPasswordIncorrect = false;

        public string Username { get => username; set => SetValue(ref username, value); }
        public string FirstName { get => firstName; set => SetValue(ref firstName, value); }
        public string Password { get => password; set => SetValue(ref password, value); }
        public string ConfirmPassword { get => confirmPassword; set => SetValue(ref confirmPassword, value); }
        public string HourRate { get => hourRate; set => SetValue(ref hourRate, value); }
        public string LastName { get => lastName; set => SetValue(ref lastName, value); }
        public string Note { get => note; set => SetValue(ref note, value); }

        public bool IsPasswordIncorrect { get => isPasswordIncorrect; set => SetValue(ref isPasswordIncorrect, value); }
        public bool IsConfirmPasswordIncorrect { get => isConfirmPasswordIncorrect; set => SetValue(ref isConfirmPasswordIncorrect, value); }

        public ICommand RegisterStudentCommand { get; }

        public RegisterStudentViewModel()
        {
            RegisterStudentCommand = new RegisterStudentCommand(this, DependencyService.Get<IUserService>());
        }
    }
}
