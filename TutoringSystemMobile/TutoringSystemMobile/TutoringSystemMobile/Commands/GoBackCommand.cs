using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands
{
    public class GoBackCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly string navigationDestination;

        public GoBackCommand(string navigationDestination)
        {
            this.navigationDestination = navigationDestination;
        }

        public bool CanExecute(object parameter)
        {
            return navigationDestination != null;
        }

        public async void Execute(object parameter)
        {
            await Shell.Current.GoToAsync($"//{navigationDestination}");
        }
    }
}
