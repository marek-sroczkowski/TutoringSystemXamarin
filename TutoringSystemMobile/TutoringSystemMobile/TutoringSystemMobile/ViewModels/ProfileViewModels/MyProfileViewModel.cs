using Rg.Plugins.Popup.Services;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.AddressViewModels;
using TutoringSystemMobile.ViewModels.PhoneNumberViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.ProfileViewModels
{
    public class MyProfileViewModel : BaseViewModel
    {
        private bool isDeactiveAccoutTabVisible;

        public bool IsDeactiveAccoutTabVisible { get => isDeactiveAccoutTabVisible; set => SetValue(ref isDeactiveAccoutTabVisible, value); }

        public Command PageAppearingCommand { get; }
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
            PageAppearingCommand = new Command(OnAppearing);
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

        private async void OnAppearing()
        {
            IsBusy = true;
            var role = await DependencyService.Get<IUserService>().GetUserRole();
            IsDeactiveAccoutTabVisible = role == Role.Tutor;
            IsBusy = false;
        }

        private async void OnEditGeneralInformations()
        {
            await Shell.Current.GoToAsync($"{nameof(EditGeneralUserInfoPage)}");
        }

        private async void OnEditAddress()
        {
            var address = await DependencyService.Get<IAddressService>().GetAddressOfLoggedInUserAsync();
            await Shell.Current.GoToAsync($"{nameof(EditAddressPage)}?{nameof(EditAddressViewModel.Id)}={address.Id}");
        }

        private async void OnEditContact()
        {
            const string contactLabel = "Ogólne dane kontaktowe";
            const string phonesLabel = "Numery telefonów";
            var result = await Shell.Current.DisplayActionSheet("Jakie informacje chcesz zaktualizować?", "Anuluj", null, contactLabel, phonesLabel);
            if (result == contactLabel)
            {
                var contact = await DependencyService.Get<IContactService>().GetContactByLoggedInUserAsync();
                await PopupNavigation.Instance.PushAsync(new EditContactPopupPage(contact.Id));
            }
            else if (result == phonesLabel)
            {
                var contact = await DependencyService.Get<IContactService>().GetContactByLoggedInUserAsync();
                await Shell.Current.GoToAsync($"{nameof(EditPhonesPage)}?{nameof(EditPhonesViewModel.ContactId)}={contact.Id}");
            }
        }

        private async void OnEditProfilePicture()
        {
            await Shell.Current.GoToAsync($"{nameof(ProfilePicturePage)}");
        }

        private void OnChangePassword()
        {

        }

        private async void OnDeactivateAccount()
        {
            var result = await Shell.Current.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć swoje konto?", "Tak", "Nie");
            if (result)
            {
                var removed = await DependencyService.Get<IUserService>().DeactivateUserAsync();
                if (removed)
                    OnLogout();
                else
                    DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj ponownie później!");
            }
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