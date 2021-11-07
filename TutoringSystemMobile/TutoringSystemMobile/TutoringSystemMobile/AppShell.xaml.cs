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

            Routing.RegisterRoute(nameof(OrderDetailsTutorPage), typeof(OrderDetailsTutorPage));
            Routing.RegisterRoute(nameof(NewOrderTutotPage), typeof(NewOrderTutotPage));
            Routing.RegisterRoute(nameof(OrderFilteringTutorPage), typeof(OrderFilteringTutorPage));
            Routing.RegisterRoute(nameof(EditOrderTutorPage), typeof(EditOrderTutorPage));

            Routing.RegisterRoute(nameof(SubjectDetailsTutorPage), typeof(SubjectDetailsTutorPage));
            Routing.RegisterRoute(nameof(NewSubjectTutorPage), typeof(NewSubjectTutorPage));
            Routing.RegisterRoute(nameof(EditSubjectTutorPage), typeof(EditSubjectTutorPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            SecureStorage.Remove("token");
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedOut.ToString());
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}