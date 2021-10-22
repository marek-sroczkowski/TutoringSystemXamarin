using System;
using System.Collections.Generic;
using TutoringSystemMobile.ViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(StartTutorPage), typeof(StartTutorPage));
            Routing.RegisterRoute(nameof(ReservationTutorPage), typeof(ReservationTutorPage));
            Routing.RegisterRoute(nameof(OrdersTutorPage), typeof(OrdersTutorPage));
            Routing.RegisterRoute(nameof(ReportTutorPage), typeof(ReportTutorPage));
            Routing.RegisterRoute(nameof(StudentsTutorPage), typeof(StudentsTutorPage));
            Routing.RegisterRoute(nameof(AccountTutorPage), typeof(AccountTutorPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
