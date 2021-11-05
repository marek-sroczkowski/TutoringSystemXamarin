using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TitledPicker : ContentView
    {
        private string placeholder = string.Empty;

        public TitledPicker()
        {
            InitializeComponent();
            //PickerContent.BindingContext = this;
            LabelTitle.BindingContext = this;
        }

        public static BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(TitledPicker), null, BindingMode.TwoWay);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static BindableProperty ItemSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IList<object>), typeof(TitledPicker), new List<object>(), BindingMode.TwoWay, null, OnItemsSourceChanged);

        public IList<object> ItemsSource
        {
            get => (IList<object>)GetValue(ItemSourceProperty);
            set => SetValue(ItemSourceProperty, value);
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = (bindable as TitledPicker).PickerContent;
            picker.Items.Clear();
            var newList = newvalue as List<string>;
            if (newvalue != null)
            {
                foreach (var item in newList)
                {
                    picker.Items.Add(item.ToString());
                }
            }
        }

        public static BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(TitledPicker), string.Empty, BindingMode.TwoWay);

        public string SelectedItem
        {
            get => (string)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(int), typeof(TitledPicker), 18, BindingMode.TwoWay);

        public int FontSize
        {
            get => (int)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        private void PickerContent_Focused(object sender, FocusEventArgs e)
        {
            LabelTitle.Text = Placeholder;
            LabelTitle.IsVisible = true;
            LabelTitle.SetAppThemeColor(Label.TextColorProperty, Color.FromHex("#2196F3"), Color.FromHex("#2196F3"));

            if (SelectedItem == null || SelectedItem.Length == 0)
            {
                placeholder = PickerContent.Title;
                PickerContent.Title = string.Empty;
            }
        }

        private void PickerContent_Unfocused(object sender, FocusEventArgs e)
        {
            if (SelectedItem == null || SelectedItem.Length == 0)
            {
                PickerContent.Title = placeholder;
                LabelTitle.IsVisible = false;
            }
            else
            {
                LabelTitle.IsVisible = true;
                LabelTitle.Text = Placeholder;
                LabelTitle.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.WhiteSmoke);
            }
        }
    }
}