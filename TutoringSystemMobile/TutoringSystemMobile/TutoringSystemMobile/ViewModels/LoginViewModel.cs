using System.Windows.Input;
using TutoringSystemMobile.Commands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels
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

        public LoginViewModel()
        {
            LoginCommand = new LoginCommand(this, DependencyService.Get<IUserService>());
        }
    }
}
