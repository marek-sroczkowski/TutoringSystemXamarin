using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            var accountStatus = await GetAccountStatus();

            switch (accountStatus)
            {
                case AccountStatus.LoggedAsTutor:
                    MessagingCenter.Send(this, Role.Tutor.ToString());
                    await Shell.Current.GoToAsync($"//{nameof(StartTutorPage)}");
                    break;
                case AccountStatus.LoggedAsStudent:
                    MessagingCenter.Send(this, Role.Student.ToString());
                    await Shell.Current.GoToAsync($"//{nameof(StartStudentPage)}");
                    break;
                case AccountStatus.InactiveAccount:
                    await Shell.Current.GoToAsync($"//{nameof(AccountActivationPage)}");
                    break;
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
