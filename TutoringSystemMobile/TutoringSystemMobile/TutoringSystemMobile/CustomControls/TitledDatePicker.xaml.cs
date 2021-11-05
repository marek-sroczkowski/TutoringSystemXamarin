using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TitledDatePicker : ContentView
    {
        public TitledDatePicker()
        {
            InitializeComponent();
            LabelTitle.BindingContext = this;
            DatePickerContent.BindingContext = this;
        }

        public static BindableProperty SelectedDateProperty =
            BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(TitledDatePicker), null, BindingMode.TwoWay);

        public DateTime? SelectedDate
        {
            get => (DateTime?)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }

        public static BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(TitledDatePicker), null, BindingMode.TwoWay);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static BindableProperty TitleColorProperty =
            BindableProperty.Create(nameof(TitleColor), typeof(string), typeof(TitledDatePicker), "#cccccc", BindingMode.TwoWay);

        public string TitleColor
        {
            get { return (string)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }

        public static BindableProperty TimespanProperty =
            BindableProperty.Create(nameof(Timespan), typeof(TimeSpan), typeof(TitledDatePicker), DateTime.Today.TimeOfDay, BindingMode.TwoWay);

        public TimeSpan Timespan
        {
            get => (TimeSpan)GetValue(TimespanProperty);
            set => SetValue(TimespanProperty, value);
        }

        public static BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(int), typeof(TitledDatePicker), 18, BindingMode.TwoWay);

        public int FontSize
        {
            get => (int)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static BindableProperty IsEditableProperty =
            BindableProperty.Create(nameof(IsEditable), typeof(bool), typeof(TitledDatePicker), true, BindingMode.TwoWay);

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        private void DatePickerContent_Focused(object sender, FocusEventArgs e)
        {
            LabelTitle.SetAppThemeColor(Label.TextColorProperty, Color.FromHex("#2196F3"), Color.FromHex("#2196F3"));
        }

        private void DatePickerContent_Unfocused(object sender, FocusEventArgs e)
        {
            LabelTitle.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.WhiteSmoke);
        }
    }
}