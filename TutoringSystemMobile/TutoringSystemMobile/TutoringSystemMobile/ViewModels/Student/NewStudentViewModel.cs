using System;
using System.Text;
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
    public class NewStudentViewModel : BaseViewModel
    {
        private string username;
        private string firstName = "";
        private string lastName = "";
        private string hourRate;
        private string note;
        private bool isUsernameLabelVisible;

        public string Username { get => username; set => SetValue(ref username, value); }
        public string FirstName { get => firstName; set => SetValue(ref firstName, value); }
        public string LastName { get => lastName; set => SetValue(ref lastName, value); }
        public string HourRate { get => hourRate; set => SetValue(ref hourRate, value); }
        public string Note { get => note; set => SetValue(ref note, value); }

        public bool IsUsernameLabelVisible { get => isUsernameLabelVisible; set => SetValue(ref isUsernameLabelVisible, value); }

        public Command CreateStudentCommand { get; }

        public NewStudentViewModel()
        {
            CreateStudentCommand = new Command(async () => await OnCreateStudent(), CanCreateStudent);
            PropertyChanged += (_, __) => CreateStudentCommand.ChangeCanExecute();
        }

        private async Task OnCreateStudent()
        {
            IsBusy = true;
            var errors = await DependencyService.Get<IUserService>().CreateStudentAsync(new NewStudentDto(FirstName, LastName, double.Parse(HourRate), Note, Username));
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
            {
                builder.AppendLine(ToastConstans.TakenLogin);
            }
            if (errors.Password != null)
            {
                builder.AppendLine(ToastConstans.IncorrectPassword);
            }

            return builder.ToString();
        }

        public bool CanCreateStudent()
        {
            if (FirstName.Length >= 3 && LastName.Length >= 3 && Username.IsEmpty())
            {
                Username = $"{FirstName[..3].ToLower()}{LastName[..3].ToLower()}{new Random().Next(100, 1000)}";
                IsUsernameLabelVisible = true;
            }

            return !Username.IsEmpty()
                && !FirstName.IsEmpty()
                && !LastName.IsEmpty()
                && double.TryParse(HourRate, out double hourRate)
                && hourRate > 0
                && !IsBusy;
        }
    }
}