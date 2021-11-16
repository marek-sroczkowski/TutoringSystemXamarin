using System;
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

        private void OnDarkMode()
        {

        }

        private void OnRateApp()
        {

        }

        private void OnLogout()
        {

        }
    }
}
