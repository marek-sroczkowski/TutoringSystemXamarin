using System.Threading.Tasks;
using System.Windows.Input;
using TutoringSystemMobile.Commands.PhoneNumberCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.PhoneNumberViewModels
{
    public class EditPhoneNumberViewModel : BaseViewModel
    {
        private long phoneNumberId;
        private string owner;
        private string number;
        private long contactId;

        public long ContactId { get => contactId; set => SetValue(ref contactId, value); }
        public long PhoneNumberId { get => phoneNumberId; set => SetValue(ref phoneNumberId, value); }
        public string Owner { get => owner; set => SetValue(ref owner, value); }
        public string Number { get => number; set => SetValue(ref number, value); }

        public Command PageAppearingCommand { get; }
        public ICommand EditPhoneNumberCommand { get; }

        private readonly IPhoneNumberService phoneNumberService;

        public EditPhoneNumberViewModel(long phoneNumberId)
        {
            PhoneNumberId = phoneNumberId;
            phoneNumberService = DependencyService.Get<IPhoneNumberService>();
            PageAppearingCommand = new Command(async () => await OnAppearing());
            EditPhoneNumberCommand = new EditPhoneNumberCommand(this, phoneNumberService);
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            ContactId = long.Parse(await SecureStorage.GetAsync("contactId"));
            var phoneNumber = await phoneNumberService.GetPhoneNumberById(ContactId, PhoneNumberId);
            Owner = phoneNumber.Owner;
            Number = phoneNumber.Number;

            IsBusy = false;
        }
    }
}