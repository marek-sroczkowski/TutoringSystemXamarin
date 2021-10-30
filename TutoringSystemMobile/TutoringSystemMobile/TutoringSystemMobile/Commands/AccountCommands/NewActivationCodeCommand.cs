using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.AccountViewModels;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.AccountCommands
{
    public class NewActivationCodeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private AccountActivationViewModel viewModel;
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
            return true;
        }

        public async void Execute(object parameter)
        {
            if (await userService.SendNewActivationTokenAsync())
                DependencyService.Get<IToast>()?.MakeToast("Wysłano nowy kod aktywacyjny!");
        }
    }
}
