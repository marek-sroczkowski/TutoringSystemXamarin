using Xamarin.Forms;

namespace TutoringSystemMobile.Helpers
{
    public static class TheTheme
    {
        public static void SetTheme()
        {
            switch (Settings.Theme)
            {
                case AppThemeMode.Light:
                    Application.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                case AppThemeMode.Dark:
                    Application.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
                case AppThemeMode.Default:
                default:
                    Application.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
            }
        }
    }
}