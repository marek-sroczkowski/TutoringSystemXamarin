﻿using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Dtos.PhoneNumber;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Contact
{
    public class ContactDetailsViewModel : BaseViewModel
    {
        private long id;
        private string owner;
        private string email;
        private string discordName;
        private bool isTutorLoggedIn;
        private bool isRefreshing;

        public long Id { get => id; set => SetValue(ref id, value); }

        public string Email
        {
            get => string.IsNullOrEmpty(email) ? GeneralConstans.NoValue : email;
            set => SetValue(ref email, value);
        }

        public string DiscordName
        {
            get => string.IsNullOrEmpty(discordName) ? GeneralConstans.NoValue : discordName;
            set => SetValue(ref discordName, value);
        }

        public string Owner { get => owner; set => SetValue(ref owner, value); }
        public bool IsTutorLoggedIn { get => isTutorLoggedIn; set => SetValue(ref isTutorLoggedIn, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetValue(ref isRefreshing, value); }

        public ObservableCollection<PhoneNumberDto> PhoneNumbers { get; }

        public Command PageAppearingCommand { get; }
        public Command LoadContactCommand { get; }
        public Command<PhoneNumberDto> CallToStudentCommand { get; }
        public Command<PhoneNumberDto> EditPhoneNumberCommand { get; }
        public Command<PhoneNumberDto> RemovePhoneNumberCommand { get; }
        public Command AddPhoneNumberCommand { get; }

        private readonly IContactService contactService;
        private readonly IPhoneNumberService phoneNumberService;

        public ContactDetailsViewModel()
        {
            GetLoggedInUserRole();

            PhoneNumbers = new ObservableCollection<PhoneNumberDto>();

            contactService = DependencyService.Get<IContactService>();
            phoneNumberService = DependencyService.Get<IPhoneNumberService>();

            PageAppearingCommand = new Command(async () => await OnAppearing());
            LoadContactCommand = new Command(async () => await OnLoadContact());
            CallToStudentCommand = new Command<PhoneNumberDto>(OnSelectNumberToCall);
            AddPhoneNumberCommand = new Command(async () => await OnAddNewPhone());
            RemovePhoneNumberCommand = new Command<PhoneNumberDto>(async (phone) => await OnRemovePhone(phone));
            EditPhoneNumberCommand = new Command<PhoneNumberDto>(async (phone) => await OnEditPhone(phone));

            MessagingCenter.Subscribe<ReloadContactService>(this, message: MessagingCenterConstans.Reload, async (sender) =>
            {
                await OnAppearing();
            });
        }

        private async Task OnLoadContact()
        {
            IsBusy = true;

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
            IsRefreshing = false;
        }

        private async void GetLoggedInUserRole()
        {
            var statusString = await SecureStorage.GetAsync(nameof(AccountStatus));
            var status = statusString.GetAccountStatus();
            IsTutorLoggedIn = status == AccountStatus.LoggedAsTutor;
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
                PhoneNumbers.Remove(phone);
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.Removed);
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
            await SecureStorage.SetAsync(SecureStorageConstans.CurrentPage, SecureStorageConstans.Contact);
            await OnLoadContact();
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