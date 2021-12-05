using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.PhoneNumberDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.ContactViewModels
{
    public class ContactDetailsViewModel : BaseViewModel
    {
        private long id;
        private string owner;
        private string email;
        private string discordName;

        public long Id { get => id; set => SetValue(ref id, value); }
        public string Email
        {
            get
            {
                if (!string.IsNullOrEmpty(email))
                    return email;
                else
                    return GeneralConstans.NoValue;
            }

            set => SetValue(ref email, value);
        }
        public string DiscordName
        {
            get
            {
                if (!string.IsNullOrEmpty(discordName))
                    return discordName;
                else
                    return GeneralConstans.NoValue;
            }

            set => SetValue(ref discordName, value);
        }
        public string Owner { get => owner; set => SetValue(ref owner, value); }

        public ObservableCollection<PhoneNumberDto> PhoneNumbers { get; }

        public Command PageAppearingCommand { get; }
        public Command<PhoneNumberDto> CallToStudentCommand { get; }
        public Command<PhoneNumberDto> EditPhoneNumberCommand { get; }
        public Command<PhoneNumberDto> RemovePhoneNumberCommand { get; }
        public Command AddPhoneNumberCommand { get; }

        private readonly IContactService contactService;
        private readonly IPhoneNumberService phoneNumberService;

        public ContactDetailsViewModel()
        {
            PhoneNumbers = new ObservableCollection<PhoneNumberDto>();

            contactService = DependencyService.Get<IContactService>();
            phoneNumberService = DependencyService.Get<IPhoneNumberService>();

            PageAppearingCommand = new Command(async () => await OnAppearing());
            CallToStudentCommand = new Command<PhoneNumberDto>(OnSelectNumberToCall);
            AddPhoneNumberCommand = new Command(async () => await OnAddNewPhone());
            RemovePhoneNumberCommand = new Command<PhoneNumberDto>(async (phone) => await OnRemovePhone(phone));
            EditPhoneNumberCommand = new Command<PhoneNumberDto>(async (phone) => await OnEditPhone(phone));

            MessagingCenter.Subscribe<ReloadContactService>(this, message: MessagingCenterConstans.Reload, async (sender) =>
            {
                await OnAppearing();
            });
        }

        private async Task OnEditPhone(PhoneNumberDto phone)
        {
            await PopupNavigation.Instance.PushAsync(new EditPhoneNumberPopupPage(phone.Id));
        }

        private async Task OnRemovePhone(PhoneNumberDto phone)
        {
            var removed = await phoneNumberService.DeletePhoneNumberAsync(Id, phone.Id);
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
            await PopupNavigation.Instance.PushAsync(new NewPhoneNumberPopupPage(Id));
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            await SecureStorage.SetAsync(SecureStorageConstans.CurrentPage, SecureStorageConstans.Contact);
            Id = long.Parse(await SecureStorage.GetAsync(SecureStorageConstans.ContactId));
            var contact = await contactService.GetContactByIdAsync(Id);

            Email = contact.Email;
            DiscordName = contact.DiscordName;
            Owner = contact.Owner;

            PhoneNumbers.Clear();
            foreach (var phone in contact.PhoneNumbers)
            {
                PhoneNumbers.Add(phone);
            }

            IsBusy = false;
        }

        private void OnSelectNumberToCall(PhoneNumberDto phone)
        {
            try
            {
                PhoneDialer.Open(phone.Number);
            }
            catch (ArgumentNullException)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.InvalidPhoneNumber);
            }
            catch (FeatureNotSupportedException)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.FeatureNotSupported);
            }
            catch (Exception)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.CanNotMakeCall);
            }
        }
    }
}