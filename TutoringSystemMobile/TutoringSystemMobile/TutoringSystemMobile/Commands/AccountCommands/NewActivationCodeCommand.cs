using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.AccountViewModels;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.AccountCommands
{
    public class NewActivationCodeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly AccountActivationViewModel viewModel;
        private readonly IUserService userService;

        public NewActivationCodeCommand(AccountActivationViewModel viewModel, IUserService userService)
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
            return !viewModel.IsBusy;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var sent = await userService.SendNewActivationTokenAsync();
            viewModel.IsBusy = false;

            if (sent)
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.SentActivationCode);
            else
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorActivationCodeSending);
        }
    }
}
