using System;
using System.Collections.ObjectModel;
using TutoringSystemMobile.Models.PhoneNumberDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
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
        public Command<PhoneNumberDto> CallToStudent { get; }

        private readonly IContactService contactService;

        public ContactDetailsViewModel()
        {
            PhoneNumbers = new ObservableCollection<PhoneNumberDto>();
            contactService = DependencyService.Get<IContactService>();
            PageAppearingCommand = new Command(OnAppearing);
            CallToStudent = new Command<PhoneNumberDto>(OnSelectNumberToCall);
        }

        private async void OnAppearing()
        {
            IsBusy = true;

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
                DependencyService.Get<IToast>()?.MakeToast("Niepoprawny numer!");
            }
            catch (FeatureNotSupportedException)
            {
                DependencyService.Get<IToast>()?.MakeToast("Na Twoim urządzeniu ta opcja nie działa!");
            }
            catch (Exception)
            {
                DependencyService.Get<IToast>()?.MakeToast("Błąd! Nie można zadzownić!");
            }
        }
    }
}