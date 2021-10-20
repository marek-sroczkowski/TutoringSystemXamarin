using System;
using System.Collections.Generic;
using System.ComponentModel;
using TutoringSystemMobile.Models;
using TutoringSystemMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}