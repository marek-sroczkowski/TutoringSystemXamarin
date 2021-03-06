using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Account;
using TutoringSystemMobile.Services.Interfaces;
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

        private readonly IUserService userService = DependencyService.Get<IUserService>();

        public NewStudentViewModel()
        {
            CreateStudentCommand = new Command(async () => await OnCreateStudent(), CanCreateStudent);
            PropertyChanged += (_, __) => CreateStudentCommand.ChangeCanExecute();
        }

        private async Task OnCreateStudent()
        {
            IsBusy = true;
            var student = new NewStudentDto(FirstName, LastName, double.Parse(HourRate), Note, Username);
            var errors = await userService.CreateStudentAsync(student);
            IsBusy = false;

            if (errors is null)
            {
                ToastHelper.MakeLongToast(ToastConstans.SuccessfulRegistration);
                await Shell.Current.GoToAsync($"//{nameof(StudentsTutorPage)}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, errors.ToString(), GeneralConstans.Ok);
            }
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