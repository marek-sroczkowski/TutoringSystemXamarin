﻿using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
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
            viewModel.IsPasswordIncorrect = !string.IsNullOrWhiteSpace(viewModel.Password) 
                && !Regex.IsMatch(viewModel.Password, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            viewModel.IsConfirmPasswordIncorrect = !string.IsNullOrWhiteSpace(viewModel.ConfirmPassword) &&
                !viewModel.Password.Equals(viewModel.ConfirmPassword);
            viewModel.IsEmailIncorrect = !string.IsNullOrWhiteSpace(viewModel.Email) &&
                !IsValidEmail(viewModel.Email);

            return !string.IsNullOrWhiteSpace(viewModel.Username) &&
                !string.IsNullOrWhiteSpace(viewModel.FirstName) &&
                !string.IsNullOrWhiteSpace(viewModel.Email) &&
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
                DependencyService.Get<IToast>()?.MakeLongToast("Rejestracja powiodła się!");
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Uwaga!", GetErrorsMessage(errors), "OK");
            }
        }

        public string GetErrorsMessage(RegisterErrorTypes errors)
        {
            StringBuilder builder = new StringBuilder("Rejestracja nie powiodła się!\n");
            if (errors.Email != null)
                builder.AppendLine("Email jest już zajęty");
            if (errors.Username != null)
                builder.AppendLine("Login jest już zajęty");
            if (errors.Password != null)
                builder.AppendLine("Nieprawidłowe hasło");

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