using Rg.Plugins.Popup.Services;
using System;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.TutorProfileViewModels
{
    public class TutorProfileViewModel : BaseViewModel
    {
        public Command EditGeneralInformationsCommand { get; }
        public Command EditAddressCommand { get; }
        public Command EditContactCommand { get; }
        public Command EditProfilePictureCommand { get; }
        public Command ChangePasswordCommand { get; }
        public Command DeactivateAccountCommand { get; }
        public Command DarkModeCommand { get; }
        public Command RateAppCommand { get; }
        public Command LogoutCommand { get; }

        public TutorProfileViewModel()
        {
            EditGeneralInformationsCommand = new Command(OnEditGeneralInformations);
            EditAddressCommand = new Command(OnEditAddress);
            EditContactCommand = new Command(OnEditContact);
            EditProfilePictureCommand = new Command(OnEditProfilePicture);
            ChangePasswordCommand = new Command(OnChangePassword);
            DeactivateAccountCommand = new Command(OnDeactivateAccount);
            DarkModeCommand = new Command(OnDarkMode);
            RateAppCommand = new Command(OnRateApp);
            LogoutCommand = new Command(OnLogout);
        }

        private void OnEditGeneralInformations()
        {

        }

        private void OnEditAddress()
        {

        }

        private void OnEditContact()
        {

        }

        private void OnEditProfilePicture()
        {

        }

        private void OnChangePassword()
        {

        }

        private void OnDeactivateAccount()
        {

        }

        private async void OnDarkMode()
        {
            await PopupNavigation.Instance.PushAsync(new AppThemePopupPage());
        }

        private async void OnRateApp()
        {
            await Launcher.OpenAsync($"http://play.google.com/store/apps/details?id=com.facebook.orca&gl=PL");
        }

        private async void OnLogout()
        {
            SecureStorage.Remove("token");
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedOut.ToString());
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}