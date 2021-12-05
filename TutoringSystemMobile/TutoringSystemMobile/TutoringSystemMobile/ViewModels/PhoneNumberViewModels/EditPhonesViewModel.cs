using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.PhoneNumberDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
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

            AddPhoneNumberCommand = new Command(async () => await OnAddNewPhone());
            RemovePhoneNumberCommand = new Command<PhoneNumberDto>(async (phone) => await OnRemovePhone(phone));
            EditPhoneNumberCommand = new Command<PhoneNumberDto>(async (phone) => await OnEditPhone(phone));

            MessagingCenter.Subscribe<ReloadContactService>(this, message: MessagingCenterConstans.Reload, (sender) =>
            {
                LoadPhonesByContactId(contactId);
            });
        }

        private async Task OnEditPhone(PhoneNumberDto phone)
        {
            await PopupNavigation.Instance.PushAsync(new EditPhoneNumberPopupPage(phone.Id));
        }

        private async Task OnRemovePhone(PhoneNumberDto phone)
        {
            var removed = await phoneNumberService.DeletePhoneNumberAsync(ContactId, phone.Id);
            if (removed)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.Removed);
                PhoneNumbers.Remove(phone);
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async Task OnAddNewPhone()
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