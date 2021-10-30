﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TitledEntry : ContentView
    {
        string placeholder = string.Empty;

        public enum KeyboardEnum
        {
            Default,
            Text,
            Chat,
            Url,
            Email,
            Telephone,
            Numeric,
        }

        public TitledEntry()
        {
            InitializeComponent();

            EntryContent.BindingContext = this;
            LabelTitle.BindingContext = this;
            LabelTitle.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.WhiteSmoke);
        }

        public static BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(TitledEntry), null, BindingMode.TwoWay);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static BindableProperty IsPasswordProperty =
            BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(TitledEntry), null, BindingMode.TwoWay);

        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        public static BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(int), typeof(TitledEntry), 16, BindingMode.TwoWay);

        public int FontSize
        {
            get => (int)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(TitledEntry), string.Empty, BindingMode.TwoWay);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(KeyboardProperty), typeof(KeyboardEnum), typeof(TitledEntry), KeyboardEnum.Default, BindingMode.TwoWay);

        public KeyboardEnum EntryKeyboard
        {
            get => (KeyboardEnum)GetValue(KeyboardProperty);
            set
            {
                SetValue(KeyboardProperty, value);
                SetKeyboard();
            }
        }

        void Handle_ContainerFocused(object sender, FocusEventArgs e)
        {
            EntryContent.Focus();
        }

        void Handle_Focused(object sender, FocusEventArgs e)
        {
            LabelTitle.Text = Placeholder;
            LabelTitle.IsVisible = true;
            LabelTitle.SetAppThemeColor(Label.TextColorProperty, Color.FromHex("#2196F3"), Color.FromHex("#2196F3"));

            if (EntryContent.Text == null || EntryContent.Text.Length == 0)
            {
                placeholder = EntryContent.Placeholder;
                EntryContent.Placeholder = string.Empty;
            }
        }

        void Handle_Unfocused(object sender, FocusEventArgs e)
        {
            if (EntryContent.Text == null || EntryContent.Text.Length == 0)
            {
                EntryContent.Placeholder = placeholder;
                LabelTitle.IsVisible = false;
            }
            else
            {
                LabelTitle.IsVisible = true;
                LabelTitle.Text = Placeholder;
                LabelTitle.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.WhiteSmoke);
            }
        }

        void Handle_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (!string.IsNullOrEmpty(entry.Text))
            {
                LabelTitle.Text = Placeholder;
            }
        }

        void SetKeyboard()
        {
            switch (EntryKeyboard)
            {
                case KeyboardEnum.Default:
                    EntryContent.Keyboard = Keyboard.Default;
                    break;
                case KeyboardEnum.Text:
                    EntryContent.Keyboard = Keyboard.Text;
                    break;
                case KeyboardEnum.Chat:
                    EntryContent.Keyboard = Keyboard.Chat;
                    break;
                case KeyboardEnum.Url:
                    EntryContent.Keyboard = Keyboard.Url;
                    break;
                case KeyboardEnum.Email:
                    EntryContent.Keyboard = Keyboard.Email;
                    break;
                case KeyboardEnum.Telephone:
                    EntryContent.Keyboard = Keyboard.Telephone;
                    break;
                case KeyboardEnum.Numeric:
                    EntryContent.Keyboard = Keyboard.Numeric;
                    break;
                default:
                    EntryContent.Keyboard = Keyboard.Default;
                    break;
            }
        }
    }
}