using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.ProfileViewModels;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.ProfileCommands
{
    public class ChangePasswordCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly ChangePasswordViewModel viewModel;
        private readonly IUserService userService;

        public ChangePasswordCommand(ChangePasswordViewModel viewModel, IUserService userService)
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
            viewModel.IsNewPasswordIncorrect = !viewModel.NewPassword.IsEmpty()
                && !Regex.IsMatch(viewModel.NewPassword, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            viewModel.IsConfirmPasswordIncorrect = !viewModel.ConfirmPassword.IsEmpty() &&
                !viewModel.NewPassword.Equals(viewModel.ConfirmPassword);

            return !viewModel.OldPassword.IsEmpty() &&
                Regex.IsMatch(viewModel.NewPassword, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$") &&
                viewModel.NewPassword.Equals(viewModel.ConfirmPassword) &&
                !viewModel.IsBusy;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var errors = await userService.ChangePasswordAsync(new PasswordDto(viewModel.NewPassword, viewModel.ConfirmPassword, viewModel.OldPassword));
            viewModel.IsBusy = false;

            if (errors is null)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Zmieniono hasło");
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Uwaga!", GetErrorsMessage(errors), "OK");
            }
        }

        private string GetErrorsMessage(IEnumerable<WrongPasswordStatus> errors)
        {
            StringBuilder builder = new StringBuilder("Zmiana hasła nie powiodła się!\n");
            foreach (var error in errors)
            {
                switch (error)
                {
                    case WrongPasswordStatus.PasswordsVary:
                        builder.AppendLine("Hasła różnią się");
                        break;
                    case WrongPasswordStatus.TooShort:
                        builder.AppendLine("Nowe hasło jest zbyt krótkie");
                        break;
                    case WrongPasswordStatus.DuplicateOfOld:
                        builder.AppendLine("Nowe hasło musi się różnić od starego");
                        break;
                    case WrongPasswordStatus.InvalidOldPassword:
                        builder.AppendLine("Niepoprawne stare hasło");
                        break;
                    case WrongPasswordStatus.InternalError:
                    default:
                        builder.AppendLine("Wewnętrzny błąd aplikacji, spróbuj ponownie później!");
                        break;
                }
            }

            return builder.ToString();
        }
    }
}