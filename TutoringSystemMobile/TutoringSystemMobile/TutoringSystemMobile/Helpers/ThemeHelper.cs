using TutoringSystemMobile.Models.Enums;
using Xamarin.Forms;

namespace TutoringSystemMobile.Helpers
{
    public static class ThemeHelper
    {
        public static void SetTheme()
        {
            Application.Current.UserAppTheme = Settings.Theme switch
            {
                AppThemeMode.Light => OSAppTheme.Light,
                AppThemeMode.Dark => OSAppTheme.Dark,
                _ => OSAppTheme.Unspecified,
            };

            var environment = DependencyService.Get<IEnvironment>();
            if (Application.Current.RequestedTheme == OSAppTheme.Dark)
            {
                environment?.SetStatusBarColor(Color.Black, false);
            }
            else
            {
                environment?.SetStatusBarColor(Color.FromHex("2196F3"), true);
            }
        }
    }
}