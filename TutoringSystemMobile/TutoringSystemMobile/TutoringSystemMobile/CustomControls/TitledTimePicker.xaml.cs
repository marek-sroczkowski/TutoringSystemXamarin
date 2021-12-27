using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TitledTimePicker : ContentView
    {
        public TitledTimePicker()
        {
            InitializeComponent();
            LabelTitle.BindingContext = this;
            TimePickerContent.BindingContext = this;
        }

        public static BindableProperty SelectedTimeProperty =
            BindableProperty.Create(nameof(SelectedTime), typeof(TimeSpan?), typeof(TitledTimePicker), null, BindingMode.TwoWay);

        public TimeSpan? SelectedTime
        {
            get => (TimeSpan?)GetValue(SelectedTimeProperty);
            set => SetValue(SelectedTimeProperty, value);
        }

        public static BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(TitledTimePicker), null, BindingMode.TwoWay);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static BindableProperty TitleColorProperty =
            BindableProperty.Create(nameof(TitleColor), typeof(string), typeof(TitledTimePicker), "#cccccc", BindingMode.TwoWay);

        public string TitleColor
        {
            get { return (string)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }

        public static BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(int), typeof(TitledTimePicker), 18, BindingMode.TwoWay);

        public int FontSize
        {
            get => (int)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static BindableProperty IsEditableProperty =
            BindableProperty.Create(nameof(IsEditable), typeof(bool), typeof(TitledTimePicker), true, BindingMode.TwoWay);

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        private void TimePickerContent_Focused(object sender, FocusEventArgs e)
        {
            LabelTitle.SetAppThemeColor(Label.TextColorProperty, Color.FromHex("#2196F3"), Color.FromHex("#2196F3"));
        }

        private void TimePickerContent_Unfocused(object sender, FocusEventArgs e)
        {
            LabelTitle.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.WhiteSmoke);
        }
    }
}