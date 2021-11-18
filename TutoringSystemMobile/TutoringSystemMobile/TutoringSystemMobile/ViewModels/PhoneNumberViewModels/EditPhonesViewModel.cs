using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using TutoringSystemMobile.Models.PhoneNumberDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.PhoneNumberViewModels
{
    [QueryProperty(nameof(ContactId), nameof(ContactId))]
    public class EditPhonesViewModel : BaseViewModel
    {
        private long contactId;

        public long ContactId
        {
            get => contactId;
            set
            {
                contactId = value;
                LoadPhonesByContactId(contactId);
            }
        }

        public ObservableCollection<PhoneNumberDto> PhoneNumbers { get; }

        public Command<PhoneNumberDto> EditPhoneNumberCommand { get; }
        public Command<PhoneNumberDto> RemovePhoneNumberCommand { get; }
        public Command AddPhoneNumberCommand { get; }

        private readonly IContactService contactService;
        private readonly IPhoneNumberService phoneNumberService;

        public EditPhonesViewModel()
        {
            PhoneNumbers = new ObservableCollection<PhoneNumberDto>();

            contactService = DependencyService.Get<IContactService>();
            phoneNumberService = DependencyService.Get<IPhoneNumberService>();

            AddPhoneNumberCommand = new Command(OnAddNewPhone);
            RemovePhoneNumberCommand = new Command<PhoneNumberDto>(OnRemovePhone);
            EditPhoneNumberCommand = new Command<PhoneNumberDto>(OnEditPhone);

            MessagingCenter.Subscribe<ReloadContactService>(this, message: "reload", (sender) =>
            {
                LoadPhonesByContactId(contactId);
            });
        }

        private async void OnEditPhone(PhoneNumberDto phone)
        {
            await PopupNavigation.Instance.PushAsync(new EditPhoneNumberPopupPage(phone.Id));
        }

        private async void OnRemovePhone(PhoneNumberDto phone)
        {
            var removed = await phoneNumberService.DeletePhoneNumberAsync(ContactId, phone.Id);
            if (removed)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Usunięto");
                PhoneNumbers.Remove(phone);
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj ponownie później!");
            }
        }

        private async void OnAddNewPhone()
        {
            await PopupNavigation.Instance.PushAsync(new NewPhoneNumberPopupPage(ContactId));
        }

        private async void LoadPhonesByContactId(long contactId)
        {
            IsBusy = true;

            var contact = await contactService.GetContactByIdAsync(contactId);
            PhoneNumbers.Clear();
            foreach (var phone in contact.PhoneNumbers)
                PhoneNumbers.Add(phone);

            IsBusy = false;
        }
    }
}