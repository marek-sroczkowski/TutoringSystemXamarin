using System;
using TutoringSystemMobile.Models.Enums;
using Xamarin.Essentials;

namespace TutoringSystemMobile.Helpers
{
    public static class Settings
    {
        private static readonly string theme = AppThemeMode.Default.ToString();
        private static readonly string loginStatus = AccountStatus.LoggedOut.ToString();
        private static readonly string baseApiUrl = "https://tutoring-system-api-app.azurewebsites.net/api/";
        private static readonly string firebaseStorageUrl = "ms-korepetytor.appspot.com";

        public static AppThemeMode Theme
        {
            get => (AppThemeMode)Enum.Parse(typeof(AppThemeMode), Preferences.Get(nameof(Theme), theme));
            set => Preferences.Set(nameof(Theme), value.ToString());
        }

        public static AccountStatus LoginStatus
        {
            get => (AccountStatus)Enum.Parse(typeof(AccountStatus), Preferences.Get(nameof(LoginStatus), loginStatus));
            set => Preferences.Set(nameof(LoginStatus), value.ToString());
        }

        public static string BaseApiUrl { get => baseApiUrl; }

        public static string FirebaseStorageUrl { get => firebaseStorageUrl; }
    }
}