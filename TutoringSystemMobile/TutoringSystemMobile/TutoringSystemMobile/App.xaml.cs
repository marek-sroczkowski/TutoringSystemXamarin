using TutoringSystemMobile.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ThemeHelper.SetTheme();
            MainPage = new AppShell();

            //CrossFirebasePushNotification.Current.OnTokenRefresh += async (s, p) =>
            //{
            //    System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            //    await DependencyService.Get<IPushNotificationTokenService>().PutTokenAsync();
            //};
            //// Push message received event
            //CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("Received");
            //};
            //CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("Opened");
            //    foreach (var data in p.Data)
            //    {
            //        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
            //    }
            //};
        }

        protected override void OnStart()
        {
            //await DependencyService.Get<IPushNotificationTokenService>().PutTokenAsync();
        }

        protected override void OnSleep()
        {
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            ThemeHelper.SetTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ThemeHelper.SetTheme();
            });
        }
    }
}