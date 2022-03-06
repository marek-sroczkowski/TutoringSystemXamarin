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

namespace TutoringSystemMobile.ViewModels.Student
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

        public Command RegisterStudentCommand { get; }

        public RegisterStudentViewModel()
        {
            RegisterStudentCommand = new Command(async () => await OnRegisterStudent(), CanRegisterStudent);
            PropertyChanged += (_, __) => RegisterStudentCommand.ChangeCanExecute();
        }

        private async Task OnRegisterStudent()
        {
            IsBusy = true;
            var errors = await DependencyService.Get<IUserService>().RegisterStudentAsync(new
                RegisterStudentDto(FirstName, LastName, double.Parse(HourRate), Note, Username, Password, ConfirmPassword));
            IsBusy = false;

            if (errors is null)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.SuccessfulRegistration);
                await Shell.Current.GoToAsync($"//{nameof(StudentsTutorPage)}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, GetErrorsMessage(errors), GeneralConstans.Ok);
            }
        }

        private string GetErrorsMessage(RegisterErrors errors)
        {
            StringBuilder builder = new StringBuilder($"{ToastConstans.RegistrationFailed}\n");
            if (errors.Username != null)
                builder.AppendLine(ToastConstans.TakenLogin);
            if (errors.Password != null)
                builder.AppendLine(ToastConstans.IncorrectPassword);

            return builder.ToString();
        }

        public bool CanRegisterStudent()
        {
            IsPasswordIncorrect = !Password.IsEmpty()
                && !Regex.IsMatch(Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            IsConfirmPasswordIncorrect = !ConfirmPassword.IsEmpty() &&
                !Password.Equals(ConfirmPassword);

            return !Username.IsEmpty() &&
                   !FirstName.IsEmpty() &&
                   Regex.IsMatch(Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$") &&
                   Password.Equals(ConfirmPassword) &&
                   double.TryParse(HourRate, out double hourRate) &&
                   hourRate > 0 &&
                   !IsBusy;
        }
    }
}