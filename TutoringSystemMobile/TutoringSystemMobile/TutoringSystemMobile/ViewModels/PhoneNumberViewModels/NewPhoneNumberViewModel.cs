using System.Windows.Input;
using TutoringSystemMobile.Commands.PhoneNumberCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.PhoneNumberViewModels
{
    public class NewPhoneNumberViewModel : BaseViewModel
    {
        private long contactId;
        private string owner;
        private string number;

        public long ContactId { get => contactId; set => SetValue(ref contactId, value); }
        public string Owner { get => owner; set => SetValue(ref owner, value); }
        public string Number { get => number; set => SetValue(ref number, value); }

        public ICommand AddPhoneNumberCommand { get; }

        public NewPhoneNumberViewModel(long contactId)
        {
            ContactId = contactId;
            AddPhoneNumberCommand = new NewPhoneNumberCommand(this, DependencyService.Get<IPhoneNumberService>());
        }
    }
}