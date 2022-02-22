﻿using TutoringSystemMobile.ViewModels.PhoneNumber;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPhoneNumberPopupPage
    {
        public NewPhoneNumberPopupPage(long contactId)
        {
            InitializeComponent();
            BindingContext = new NewPhoneNumberViewModel(contactId);
        }
    }
}