using System;
using System.Diagnostics;
using System.Windows.Input;
using TutoringSystemMobile.Commands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels
{
    public class AccountActivationViewModel : BaseViewModel
    {
        private string activationToken;
        public string ActivationToken
        {
            get => activationToken;
            set => SetValue(ref activationToken, value);
        }

        public ICommand ActivateAccountCommand { get; }
        public ICommand NewActivationCodeCommand { get; }

        public AccountActivationViewModel()
        {
            ActivateAccountCommand = new ActivateAccountCommand(this, DependencyService.Get<IUserService>());
            NewActivationCodeCommand = new NewActivationCodeCommand(this, DependencyService.Get<IUserService>());

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Debug.WriteLine("Hello");
                return true;
            });
        }
    }
}
