using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Models.Errors;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.StudentViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.StudentCommands
{
    public class RegisterStudentCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly RegisterStudentViewModel viewModel;
        private readonly IUserService userService;

        public RegisterStudentCommand(RegisterStudentViewModel viewModel, IUserService userService)
        {
            this.viewModel = viewModel;
            this.userService = userService;
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            viewModel.IsPasswordIncorrect = !viewModel.Password.IsEmpty()
                && !Regex.IsMatch(viewModel.Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            viewModel.IsConfirmPasswordIncorrect = !viewModel.ConfirmPassword.IsEmpty() &&
                !viewModel.Password.Equals(viewModel.ConfirmPassword);

            return !viewModel.Username.IsEmpty() &&
                !viewModel.FirstName.IsEmpty() &&
                Regex.IsMatch(viewModel.Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$") &&
                viewModel.Password.Equals(viewModel.ConfirmPassword) &&
                double.TryParse(viewModel.HourRate, out double hourRate) &&
                hourRate > 0 &&
                !viewModel.IsBusy;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var errors = await userService.RegisterStudentAsync(new RegisterStudentDto(viewModel.FirstName, viewModel.LastName, double.Parse(viewModel.HourRate), viewModel.Note, viewModel.Username, viewModel.Password, viewModel.ConfirmPassword));
            viewModel.IsBusy = false;

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

        private string GetErrorsMessage(RegisterErrorTypes errors)
        {
            StringBuilder builder = new StringBuilder($"{ToastConstans.RegistrationFailed}\n");
            if (errors.Username != null)
                builder.AppendLine(ToastConstans.TakenLogin);
            if (errors.Password != null)
                builder.AppendLine(ToastConstans.IncorrectPassword);

            return builder.ToString();
        }
    }
}