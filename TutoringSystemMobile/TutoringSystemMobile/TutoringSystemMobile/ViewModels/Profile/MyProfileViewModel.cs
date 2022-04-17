using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
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

        private readonly IAddressService addressService = DependencyService.Get<IAddressService>();
        private readonly IContactService contactService = DependencyService.Get<IContactService>();
        private readonly IUserService userService = DependencyService.Get<IUserService>();

        public MyProfileViewModel()
        {
            EditGeneralInformationsCommand = new Command(async () => await OnEditGeneralInformations());
            EditAddressCommand = new Command(async () => await OnEditAddress());
            EditContactCommand = new Command(async () => await OnEditContact());
            EditProfilePictureCommand = new Command(async () => await OnEditProfilePicture());
            ChangePasswordCommand = new Command(async () => await OnChangePassword());
            DeactivateAccountCommand = new Command(async () => await OnDeactivateAccount());
            DarkModeCommand = new Command(async () => await OnDarkMode());
            RateAppCommand = new Command(async () => await OnRateApp());
            LogoutCommand = new Command(OnLogout);

            OnAppearing();
        }

        private void OnAppearing()
        {
            IsBusy = true;
            IsDeactiveAccoutTabVisible = Settings.LoginStatus == AccountStatus.LoggedAsTutor;
            IsBusy = false;
        }

        private async Task OnEditGeneralInformations()
        {
            await Shell.Current.GoToAsync($"{nameof(EditGeneralUserInfoPage)}");
        }

        private async Task OnEditAddress()
        {
            var address = await addressService.GetAddressOfLoggedInUserAsync();
            await Shell.Current.GoToAsync($"{nameof(EditAddressPage)}?{nameof(EditAddressViewModel.Id)}={address.Id}");
        }

        private async Task OnEditContact()
        {
            var result = await Shell.Current.DisplayActionSheet(AlertConstans.InformationToUpdate, GeneralConstans.Cancel, null, AlertConstans.GeneralContactDetails, AlertConstans.PhoneNumbers);

            if (result == AlertConstans.GeneralContactDetails)
            {
                var contact = await contactService.GetContactByLoggedInUserAsync();
                await PopupNavigation.Instance.PushAsync(new EditContactPopupPage(contact.Id));
            }
            else if (result == AlertConstans.PhoneNumbers)
            {
                var contact = await contactService.GetContactByLoggedInUserAsync();
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
                await TryDeactivateUserAsync();
            }
        }

        private async Task TryDeactivateUserAsync()
        {
            var removed = await userService.DeactivateUserAsync();

            if (removed)
            {
                OnLogout();
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
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

        private void OnLogout()
        {
            AuthenticationHelper.Logout();
        }
    }
}