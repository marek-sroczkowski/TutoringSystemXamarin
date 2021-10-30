using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
    }
}
