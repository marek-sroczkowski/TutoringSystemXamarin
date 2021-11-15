using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
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
            viewModel.IsPasswordIncorrect = !string.IsNullOrEmpty(viewModel.Password)
                && !Regex.IsMatch(viewModel.Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            viewModel.IsConfirmPasswordIncorrect = !string.IsNullOrEmpty(viewModel.ConfirmPassword) &&
                !viewModel.Password.Equals(viewModel.ConfirmPassword);

            return !string.IsNullOrEmpty(viewModel.Username) &&
                !string.IsNullOrEmpty(viewModel.FirstName) &&
                Regex.IsMatch(viewModel.Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$") &&
                viewModel.Password.Equals(viewModel.ConfirmPassword) &&
                double.TryParse(viewModel.HourRate, out double hourRate) &&
                hourRate > 0;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var errors = await userService.RegisterStudentAsync(new RegisterStudentDto(viewModel.FirstName, viewModel.LastName, double.Parse(viewModel.HourRate), viewModel.Note, viewModel.Username, viewModel.Password, viewModel.ConfirmPassword));
            viewModel.IsBusy = false;

            if (errors is null)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Rejestracja powiodła się!");
                await Shell.Current.GoToAsync($"//{nameof(StudentsTutorPage)}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Uwaga!", GetErrorsMessage(errors), "OK");
            }
        }

        private string GetErrorsMessage(RegisterErrorTypes errors)
        {
            StringBuilder builder = new StringBuilder("Rejestracja nie powiodła się!\n");
            if (errors.Username != null)
                builder.AppendLine("Login jest już zajęty");
            if (errors.Password != null)
                builder.AppendLine("Nieprawidłowe hasło");

            return builder.ToString();
        }
    }
}