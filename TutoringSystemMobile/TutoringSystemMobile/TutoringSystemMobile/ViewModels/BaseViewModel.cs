using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TutoringSystemMobile.Helpers;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set => SetValue(ref isBusy, value);
        }

        protected void SetValue<T>(ref T privateField, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(privateField, value))
            {
                return;
            }

            privateField = value;
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Color tabBackgroundColor;
        public Color TabBackgroundColor { get => tabBackgroundColor; set => SetValue(ref tabBackgroundColor, value); }

        protected async Task SetFocusByBackgroundColor()
        {
            await Task.Run(async () =>
            {
                if (Settings.Theme == AppThemeMode.Dark)
                    TabBackgroundColor = Color.FromHex("1f1f1f");
                else if (Settings.Theme == AppThemeMode.Light)
                    TabBackgroundColor = Color.FromHex("dedede");

                await Task.Delay(100);
                TabBackgroundColor = Color.Transparent;
            });
        }
    }
}
