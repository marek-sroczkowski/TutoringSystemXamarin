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

            var e = DependencyService.Get<IEnvironment>();
            if (Application.Current.RequestedTheme == OSAppTheme.Dark)
            {
                e?.SetStatusBarColor(Color.Black, false);
            }
            else
            {
                e?.SetStatusBarColor(Color.FromHex("2196F3"), true);
            }
        }
    }
}