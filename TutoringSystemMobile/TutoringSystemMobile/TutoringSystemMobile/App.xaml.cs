using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services;
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
            flyoutItemService = DependencyService.Get<IFlyoutItemService>();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
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
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    break;
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private async Task<AccountStatus> GetAccountStatus()
        {
            var accountStatus = await SecureStorage.GetAsync(nameof(AccountStatus));
            if (string.IsNullOrEmpty(accountStatus))
                return AccountStatus.LoggedOut;

            return (AccountStatus)Enum.Parse(typeof(AccountStatus), accountStatus);
        }
    }
}
