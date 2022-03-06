﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactDetailsPage : ContentPage
    {
        public ContactDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PhonesCollection.HeightRequest = ContactPage.Height - ContactStackLayout.Height - 60;
        }
    }
}