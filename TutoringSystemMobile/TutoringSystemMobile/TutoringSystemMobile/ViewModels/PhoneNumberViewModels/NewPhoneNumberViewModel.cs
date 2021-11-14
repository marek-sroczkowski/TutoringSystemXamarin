using System.Windows.Input;
using TutoringSystemMobile.Commands.PhoneNumberCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.PhoneNumberViewModels
{
    public class NewPhoneNumberViewModel : BaseViewModel
    {
        private string owner;
        private string number;

        public long ContactId { get; set; }
        public string Owner { get => owner; set => SetValue(ref owner, value); }
        public string Number { get => number; set => SetValue(ref number, value); }

        public Command PageAppearingCommand { get; }
        public ICommand AddPhoneNumberCommand { get; }

        public NewPhoneNumberViewModel()
        {
            PageAppearingCommand = new Command(OnAppearing);
            AddPhoneNumberCommand = new NewPhoneNumberCommand(this, DependencyService.Get<IPhoneNumberService>());
        }

        private async void OnAppearing()
        {
            ContactId = long.Parse(await SecureStorage.GetAsync("contactId"));
        }
    }
}