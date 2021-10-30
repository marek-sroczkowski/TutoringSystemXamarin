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

            Routing.RegisterRoute(nameof(StartTutorPage), typeof(StartTutorPage));
            Routing.RegisterRoute(nameof(ReservationTutorPage), typeof(ReservationTutorPage));
            Routing.RegisterRoute(nameof(OrdersTutorPage), typeof(OrdersTutorPage));
            Routing.RegisterRoute(nameof(ReportTutorPage), typeof(ReportTutorPage));
            Routing.RegisterRoute(nameof(StudentsTutorPage), typeof(StudentsTutorPage));
            Routing.RegisterRoute(nameof(ProfileTutorPage), typeof(ProfileTutorPage));
            Routing.RegisterRoute(nameof(RegisterTutorPage), typeof(RegisterTutorPage));
            Routing.RegisterRoute(nameof(OrderDetailsPage), typeof(OrderDetailsPage));
            Routing.RegisterRoute(nameof(NewOrderPage), typeof(NewOrderPage));
            Routing.RegisterRoute(nameof(OrderFilteringPage), typeof(OrderFilteringPage));

            Routing.RegisterRoute(nameof(StartStudentPage), typeof(StartStudentPage));
            Routing.RegisterRoute(nameof(ReservationStudentPage), typeof(ReservationStudentPage));
            Routing.RegisterRoute(nameof(TutorsStudentPage), typeof(TutorsStudentPage));
            Routing.RegisterRoute(nameof(ProfileStudentPage), typeof(ProfileStudentPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            SecureStorage.Remove("token");
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedOut.ToString());
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}