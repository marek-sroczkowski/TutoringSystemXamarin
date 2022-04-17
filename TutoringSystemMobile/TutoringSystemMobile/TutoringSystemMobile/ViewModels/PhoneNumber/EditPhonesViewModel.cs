using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.PhoneNumber;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.PhoneNumber
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

        private readonly IContactService contactService = DependencyService.Get<IContactService>();
        private readonly IPhoneNumberService phoneNumberService = DependencyService.Get<IPhoneNumberService>();

        public EditPhonesViewModel()
        {
            PhoneNumbers = new ObservableCollection<PhoneNumberDto>();

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
                ToastHelper.MakeLongToast(ToastConstans.Removed);
                PhoneNumbers.Remove(phone);
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
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
            contact.PhoneNumbers.ToList().ForEach(phone => PhoneNumbers.Add(phone));

            IsBusy = false;
        }
    }
}