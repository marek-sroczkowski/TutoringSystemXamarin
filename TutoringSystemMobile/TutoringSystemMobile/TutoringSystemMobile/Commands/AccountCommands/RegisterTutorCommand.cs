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
using TutoringSystemMobile.ViewModels.AccountViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.AccountCommands
{
    public class RegisterTutorCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly RegisterTutorViewModel viewModel;
        private readonly IUserService userService;

        public RegisterTutorCommand(RegisterTutorViewModel viewModel, IUserService userService)
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
            viewModel.IsEmailIncorrect = !viewModel.Email.IsEmpty() &&
                !IsValidEmail(viewModel.Email);

            return !viewModel.Username.IsEmpty() &&
                !viewModel.FirstName.IsEmpty() &&
                !viewModel.Email.IsEmpty() &&
                Regex.IsMatch(viewModel.Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$") &&
                IsValidEmail(viewModel.Email) &&
                viewModel.Password.Equals(viewModel.ConfirmPassword) &&
                !viewModel.IsBusy;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var errors = await userService.RegisterTutorAsync(new RegisterTutorDto(viewModel.Username, viewModel.FirstName, viewModel.Email, viewModel.Password, viewModel.ConfirmPassword));
            viewModel.IsBusy = false;

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