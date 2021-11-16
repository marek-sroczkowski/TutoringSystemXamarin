using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile
{
    public partial class App : Application
    {
        private readonly IFlyoutItemService flyoutItemService;

        public App()
        {
            InitializeComponent();
            TheTheme.SetTheme();
            flyoutItemService = DependencyService.Get<IFlyoutItemService>();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            await NavigateByLoginStatus();
        }

        protected override void OnSleep()
        {
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            TheTheme.SetTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TheTheme.SetTheme();
            });
        }

        private async Task NavigateByLoginStatus()
        {
            var accountStatus = await GetAccountStatus();

            switch (accountStatus)
            {
                case AccountStatus.LoggedAsTutor:
                    flyoutItemService.EnableTutorFlyoutItems();
                    await Shell.Current.GoToAsync($"//{nameof(StartTutorPage)}");
                    break;
                case AccountStatus.LoggedAsStudent:
                    flyoutItemService.EnableStudentFlyoutItems();
                    await Shell.Current.GoToAsync($"//{nameof(StartStudentPage)}");
                    break;
                case AccountStatus.InactiveAccount:
                case AccountStatus.LoggedOut:
                default:
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    break;
            }
}

        private async Task<AccountStatus> GetAccountStatus()
        {
            var accountStatus = await SecureStorage.GetAsync(nameof(AccountStatus));
            return string.IsNullOrEmpty(accountStatus)
                ? AccountStatus.LoggedOut
                : (AccountStatus)Enum.Parse(typeof(AccountStatus), accountStatus);
        }
    }
}