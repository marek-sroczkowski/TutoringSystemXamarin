using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TitledCheckbox : ContentView
    {
        public TitledCheckbox()
        {
            InitializeComponent();
            CheckBoxContent.BindingContext = this;
            LabelTitle.BindingContext = this;
        }

        public static BindableProperty IsCheckedProperty =
            BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(TitledCheckbox), false, BindingMode.TwoWay);

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(TitledCheckbox), string.Empty, BindingMode.TwoWay);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            IsChecked = !IsChecked;
        }
    }
}