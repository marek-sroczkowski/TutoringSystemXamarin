using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Enums;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Profile
{
    public class AppThemeViewModel : BaseViewModel
    {
        private bool isDarkModeSelected;
        private bool isLightModeSelected;
        private bool isAutomaticModeSelected;

        public bool IsDarkModeSelected { get => isDarkModeSelected; set => SetValue(ref isDarkModeSelected, value); }
        public bool IsLightModeSelected { get => isLightModeSelected; set => SetValue(ref isLightModeSelected, value); }
        public bool IsAutomaticModeSelected { get => isAutomaticModeSelected; set => SetValue(ref isAutomaticModeSelected, value); }

        public Command DarkModeCommand { get; }
        public Command LightModeCommand { get; }
        public Command AutomaticModeCommand { get; }

        public AppThemeViewModel()
        {
            LoadAppTheme();
            DarkModeCommand = new Command(OnDarkMode);
            LightModeCommand = new Command(OnLightMode);
            AutomaticModeCommand = new Command(OnAutomaticMode);
        }

        private void LoadAppTheme()
        {
            switch (Settings.Theme)
            {
                case AppThemeMode.Light:
                    IsLightModeSelected = true;
                    break;
                case AppThemeMode.Dark:
                    IsDarkModeSelected = true;
                    break;
                case AppThemeMode.Default:
                default:
                    IsAutomaticModeSelected = true;
                    break;
            }
        }

        private void OnAutomaticMode(object obj)
        {
            SetMode(AppThemeMode.Default);
        }

        private void OnLightMode(object obj)
        {
            SetMode(AppThemeMode.Light);
        }

        private void OnDarkMode(object obj)
        {
            SetMode(AppThemeMode.Dark);
        }

        private void SetMode(AppThemeMode mode)
        {
            switch (mode)
            {
                case AppThemeMode.Light:
                    SetLightMode();
                    break;
                case AppThemeMode.Dark:
                    SetDarkMode();
                    break;
                case AppThemeMode.Default:
                default:
                    SetAutomaticMode();
                    break;
            }

            Settings.Theme = mode;
            ThemeHelper.SetTheme();
        }

        private void SetAutomaticMode()
        {
            IsAutomaticModeSelected = true;
            IsDarkModeSelected = false;
            IsLightModeSelected = false;
        }

        private void SetLightMode()
        {
            IsAutomaticModeSelected = false;
            IsDarkModeSelected = false;
            IsLightModeSelected = true;
        }

        private void SetDarkMode()
        {
            IsAutomaticModeSelected = false;
            IsDarkModeSelected = true;
            IsLightModeSelected = false;
        }
    }
}