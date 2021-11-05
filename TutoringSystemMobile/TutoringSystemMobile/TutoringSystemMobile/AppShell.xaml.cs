using System;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegisterTutorPage), typeof(RegisterTutorPage));
            Routing.RegisterRoute(nameof(OrderDetailsPage), typeof(OrderDetailsPage));
            Routing.RegisterRoute(nameof(NewOrderPage), typeof(NewOrderPage));
            Routing.RegisterRoute(nameof(OrderFilteringPage), typeof(OrderFilteringPage));
            Routing.RegisterRoute(nameof(EditOrderPage), typeof(EditOrderPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            SecureStorage.Remove("token");
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedOut.ToString());
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}