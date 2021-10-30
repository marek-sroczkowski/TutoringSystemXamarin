using System.Windows.Input;
using TutoringSystemMobile.Commands.AccountCommands;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.AccountViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string username;
        private string password;

        public string Username
        {
            get => username;
            set => SetValue(ref username, value);
        }

        public string Password
        {
            get => password;
            set => SetValue(ref password, value);
        }

        public ICommand LoginCommand { get; }
        public Command RegisterTutorCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new LoginCommand(this, DependencyService.Get<IUserService>());
            RegisterTutorCommand = new Command(OnRegisterFormClick);
        }

        public async void OnRegisterFormClick()
        {
            if (!IsBusy)
                await Shell.Current.GoToAsync($"{nameof(RegisterTutorPage)}");
        }
    }
}
