using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.ViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands
{
    public class NewRegisterTutorFormCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly LoginViewModel viewModel;

        public NewRegisterTutorFormCommand(LoginViewModel viewModel)
        {
            this.viewModel = viewModel;
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
            if (!viewModel.IsBusy)
                await Shell.Current.GoToAsync($"//{nameof(RegisterTutorPage)}");
        }
    }
}
