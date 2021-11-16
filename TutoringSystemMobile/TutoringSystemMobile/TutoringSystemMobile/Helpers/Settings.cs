using System;
using Xamarin.Essentials;

namespace TutoringSystemMobile.Helpers
{
    public static class Settings
    {
        //0 - default, 1 - light, 2 - dark
        private static readonly string theme = AppThemeMode.Default.ToString();
        public static AppThemeMode Theme
        {
            get => (AppThemeMode)Enum.Parse(typeof(AppThemeMode), Preferences.Get(nameof(Theme), theme));
            set => Preferences.Set(nameof(Theme), value.ToString());
        }
    }
}