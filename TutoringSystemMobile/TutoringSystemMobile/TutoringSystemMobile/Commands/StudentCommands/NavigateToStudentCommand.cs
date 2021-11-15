using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.ViewModels.AddressViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.StudentCommands
{
    public class NavigateToStudentCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly AddressDetailsViewModel viewModel;

        public NavigateToStudentCommand(AddressDetailsViewModel viewModel)
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
            return !viewModel.Street.IsEmpty() &&
                !viewModel.HouseAndFlatNumber.IsEmpty() &&
                !viewModel.City.IsEmpty() &&
                !viewModel.PostalCode.IsEmpty();
        }

        public async void Execute(object parameter)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                await Launcher.OpenAsync($"http://maps.apple.com/?daddr={GetDestination()},+CA&saddr=");
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                await Launcher.OpenAsync($"http://maps.google.com/?daddr={GetDestination()},+CA&saddr=");
            }
        }

        private string GetDestination()
        {
            string houseNumber = viewModel.HouseAndFlatNumber;
            if (viewModel.HouseAndFlatNumber.Contains("\\"))
            {
                int endIndex = viewModel.HouseAndFlatNumber.IndexOf("\\");
                houseNumber = viewModel.HouseAndFlatNumber.Substring(0, endIndex);
            }
            else if (viewModel.HouseAndFlatNumber.Contains("/"))
            {
                int endIndex = viewModel.HouseAndFlatNumber.IndexOf("/");
                houseNumber = viewModel.HouseAndFlatNumber.Substring(0, endIndex);
            }

            return $"{houseNumber}+{viewModel.Street}+{viewModel.PostalCode}+{viewModel.City}";
        }
    }
}
