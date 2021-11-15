using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
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
                    return "- ";
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
                    return "- ";
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

            PageAppearingCommand = new Command(OnAppearing);
            CallToStudentCommand = new Command<PhoneNumberDto>(OnSelectNumberToCall);
            AddPhoneNumberCommand = new Command(OnAddNewPhone);
            RemovePhoneNumberCommand = new Command<PhoneNumberDto>(OnRemovePhone);
            EditPhoneNumberCommand = new Command<PhoneNumberDto>(OnEditPhone);

            MessagingCenter.Subscribe<ReloadContactService>(this, message: "reload", (sender) =>
            {
                OnAppearing();
            });
        }

        private async void OnEditPhone(PhoneNumberDto phone)
        {
            await PopupNavigation.Instance.PushAsync(new EditPhoneNumberPopupPage(phone.Id));
        }

        private async void OnRemovePhone(PhoneNumberDto phone)
        {
            var removed = await phoneNumberService.DeletePhoneNumberAsync(Id, phone.Id);
            if(removed)
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
            await PopupNavigation.Instance.PushAsync(new NewPhoneNumberPopupPage());
        }

        private async void OnAppearing()
        {
            IsBusy = true;

            await SecureStorage.SetAsync("currentPage", "contact");
            long contactId = long.Parse(await SecureStorage.GetAsync("contactId"));
            var contact = await contactService.GetContactByIdAsync(contactId);

            Email = contact.Email;
            DiscordName = contact.DiscordName;
            Owner = contact.Owner;

            PhoneNumbers.Clear();
            foreach (var phone in contact.PhoneNumbers)
                PhoneNumbers.Add(phone);

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
                DependencyService.Get<IToast>()?.MakeLongToast("Niepoprawny numer!");
            }
            catch (FeatureNotSupportedException)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Na Twoim urządzeniu ta opcja nie działa!");
            }
            catch (Exception)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Nie można zadzownić! Sprawdź poprawność numeru!");
            }
        }
    }
}