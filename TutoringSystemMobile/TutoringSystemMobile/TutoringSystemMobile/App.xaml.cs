﻿using Plugin.FirebasePushNotification;
using System.Threading.Tasks;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Dtos.PushNotificationToken;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile
{
    public partial class App : Application
    {
        private readonly IFlyoutService flyoutItemService;

        public App()
        {
            InitializeComponent();
            ThemeHelper.SetTheme();
            flyoutItemService = DependencyService.Get<IFlyoutService>();
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

        protected override async void OnStart()
        {
            //await DependencyService.Get<IPushNotificationTokenService>().PutTokenAsync();
            await NavigateByLoginStatus();
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

        private async Task NavigateByLoginStatus()
        {
            switch (Settings.LoginStatus)
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
    }
}