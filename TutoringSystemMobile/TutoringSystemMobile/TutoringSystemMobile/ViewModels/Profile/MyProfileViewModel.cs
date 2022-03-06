﻿using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.Address;
using TutoringSystemMobile.ViewModels.PhoneNumber;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Profile
{
    public class MyProfileViewModel : BaseViewModel
    {
        private bool isDeactiveAccoutTabVisible;

        public bool IsDeactiveAccoutTabVisible { get => isDeactiveAccoutTabVisible; set => SetValue(ref isDeactiveAccoutTabVisible, value); }

        public Command EditGeneralInformationsCommand { get; }
        public Command EditAddressCommand { get; }
        public Command EditContactCommand { get; }
        public Command EditProfilePictureCommand { get; }
        public Command ChangePasswordCommand { get; }
        public Command DeactivateAccountCommand { get; }
        public Command DarkModeCommand { get; }
        public Command RateAppCommand { get; }
        public Command LogoutCommand { get; }

        public MyProfileViewModel()
        {
            OnAppearing();
            EditGeneralInformationsCommand = new Command(async () => await OnEditGeneralInformations());
            EditAddressCommand = new Command(async () => await OnEditAddress());
            EditContactCommand = new Command(async () => await OnEditContact());
            EditProfilePictureCommand = new Command(async () => await OnEditProfilePicture());
            ChangePasswordCommand = new Command(async () => await OnChangePassword());
            DeactivateAccountCommand = new Command(async () => await OnDeactivateAccount());
            DarkModeCommand = new Command(async () => await OnDarkMode());
            RateAppCommand = new Command(async () => await OnRateApp());
            LogoutCommand = new Command(async () => await OnLogout());
        }

        private async void OnAppearing()
        {
            IsBusy = true;
            var roleString = await SecureStorage.GetAsync($"{nameof(AccountStatus)}");
            var role = roleString.GetAccountStatus();
            IsDeactiveAccoutTabVisible = role == AccountStatus.LoggedAsTutor;
            IsBusy = false;
        }

        private async Task OnEditGeneralInformations()
        {
            await Shell.Current.GoToAsync($"{nameof(EditGeneralUserInfoPage)}");
        }

        private async Task OnEditAddress()
        {
            var address = await DependencyService.Get<IAddressService>().GetAddressOfLoggedInUserAsync();
            await Shell.Current.GoToAsync($"{nameof(EditAddressPage)}?{nameof(EditAddressViewModel.Id)}={address.Id}");
        }

        private async Task OnEditContact()
        {
            var result = await Shell.Current.DisplayActionSheet(AlertConstans.InformationToUpdate, GeneralConstans.Cancel, null, AlertConstans.GeneralContactDetails, AlertConstans.PhoneNumbers);
            if (result == AlertConstans.GeneralContactDetails)
            {
                var contact = await DependencyService.Get<IContactService>().GetContactByLoggedInUserAsync();
                await PopupNavigation.Instance.PushAsync(new EditContactPopupPage(contact.Id));
            }
            else if (result == AlertConstans.PhoneNumbers)
            {
                var contact = await DependencyService.Get<IContactService>().GetContactByLoggedInUserAsync();
                await Shell.Current.GoToAsync($"{nameof(EditPhonesPage)}?{nameof(EditPhonesViewModel.ContactId)}={contact.Id}");
            }
        }

        private async Task OnEditProfilePicture()
        {
            await Shell.Current.GoToAsync($"{nameof(ProfilePicturePage)}");
        }

        private async Task OnChangePassword()
        {
            await PopupNavigation.Instance.PushAsync(new ChangePasswordPopupPage());
        }

        private async Task OnDeactivateAccount()
        {
            var result = await Shell.Current.DisplayAlert(AlertConstans.Confirmation, AlertConstans.AccountDeletionConfirmation, GeneralConstans.Yes, GeneralConstans.No);
            if (result)
            {
                var removed = await DependencyService.Get<IUserService>().DeactivateUserAsync();
                if (removed)
                    await OnLogout();
                else
                    DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async Task OnDarkMode()
        {
            await PopupNavigation.Instance.PushAsync(new AppThemePopupPage());
        }

        private async Task OnRateApp()
        {
            await Launcher.OpenAsync($"http://play.google.com/store/apps/details?id=com.facebook.orca&gl=PL");
        }

        private async Task OnLogout()
        {
            SecureStorage.Remove(SecureStorageConstans.Token);
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedOut.ToString());
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}