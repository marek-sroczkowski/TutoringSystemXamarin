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
            Routing.RegisterRoute(nameof(EditOrderTutorPage), typeof(EditOrderTutorPage));

            Routing.RegisterRoute(nameof(SubjectDetailsTutorPage), typeof(SubjectDetailsTutorPage));
            Routing.RegisterRoute(nameof(NewSubjectTutorPage), typeof(NewSubjectTutorPage));
            Routing.RegisterRoute(nameof(EditSubjectTutorPage), typeof(EditSubjectTutorPage));

            Routing.RegisterRoute(nameof(StudentDetailsTutorPage), typeof(StudentDetailsTutorPage));
            Routing.RegisterRoute(nameof(NewExistingStudentTutorPage), typeof(NewExistingStudentTutorPage));
            Routing.RegisterRoute(nameof(CreatingNewStudentTutorPage), typeof(CreatingNewStudentTutorPage));

            Routing.RegisterRoute(nameof(EditStudentGeneralInformationTutorPage), typeof(EditStudentGeneralInformationTutorPage));
            Routing.RegisterRoute(nameof(EditContactPopupPage), typeof(EditContactPopupPage));
            Routing.RegisterRoute(nameof(EditAddressPage), typeof(EditAddressPage));
            Routing.RegisterRoute(nameof(EditPhonesPage), typeof(EditPhonesPage));
            Routing.RegisterRoute(nameof(EditGeneralUserInfoPage), typeof(EditGeneralUserInfoPage));
            Routing.RegisterRoute(nameof(ProfilePicturePage), typeof(ProfilePicturePage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            SecureStorage.Remove("token");
            await SecureStorage.SetAsync(nameof(AccountStatus), AccountStatus.LoggedOut.ToString());
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}