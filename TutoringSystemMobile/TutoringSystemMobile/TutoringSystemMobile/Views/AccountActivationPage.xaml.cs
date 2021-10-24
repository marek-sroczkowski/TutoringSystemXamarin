﻿using TutoringSystemMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountActivationPage : ContentPage
    {
        public AccountActivationPage()
        {
            InitializeComponent();
            BindingContext = new AccountActivationViewModel();
        }
    }
}