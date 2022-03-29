using System;
using Xamarin.Essentials;

namespace TutoringSystemMobile.Helpers
{
    public static class Settings
    {
        //0 - default, 1 - light, 2 - dark
        private static readonly string theme = AppThemeMode.Default.ToString();
        private static readonly string baseApiUrl = "https://tutoring-system-api-app.azurewebsites.net/api/";
        private static readonly string firebaseStorageUrl = "ms-korepetytor.appspot.com";

        public static AppThemeMode Theme
        {
            get => (AppThemeMode)Enum.Parse(typeof(AppThemeMode), Preferences.Get(nameof(Theme), theme));
            set => Preferences.Set(nameof(Theme), value.ToString());
        }

        public static string BaseApiUrl { get => baseApiUrl; }

        public static string FirebaseStorageUrl { get => firebaseStorageUrl; }
    }
}