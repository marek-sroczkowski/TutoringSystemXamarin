using System;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
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
            Routing.RegisterRoute(nameof(RegisterStudentPage), typeof(RegisterStudentPage));

            Routing.RegisterRoute(nameof(OrderDetailsTutorPage), typeof(OrderDetailsTutorPage));
            Routing.RegisterRoute(nameof(NewOrderTutotPage), typeof(NewOrderTutotPage));
            Routing.RegisterRoute(nameof(EditOrderTutorPage), typeof(EditOrderTutorPage));

            Routing.RegisterRoute(nameof(SubjectDetailsTutorPage), typeof(SubjectDetailsTutorPage));
            Routing.RegisterRoute(nameof(NewSubjectTutorPage), typeof(NewSubjectTutorPage));
            Routing.RegisterRoute(nameof(EditSubjectTutorPage), typeof(EditSubjectTutorPage));

            Routing.RegisterRoute(nameof(StudentDetailsTutorPage), typeof(StudentDetailsTutorPage));
            Routing.RegisterRoute(nameof(CreatingNewStudentTutorPage), typeof(CreatingNewStudentTutorPage));
            Routing.RegisterRoute(nameof(StudentRequestsTutorPage), typeof(StudentRequestsTutorPage));
            Routing.RegisterRoute(nameof(SearchStudentPage), typeof(SearchStudentPage));

            Routing.RegisterRoute(nameof(EditStudentGeneralInformationTutorPage), typeof(EditStudentGeneralInformationTutorPage));
            Routing.RegisterRoute(nameof(EditContactPopupPage), typeof(EditContactPopupPage));
            Routing.RegisterRoute(nameof(EditAddressPage), typeof(EditAddressPage));
            Routing.RegisterRoute(nameof(EditPhonesPage), typeof(EditPhonesPage));
            Routing.RegisterRoute(nameof(EditGeneralUserInfoPage), typeof(EditGeneralUserInfoPage));
            Routing.RegisterRoute(nameof(ProfilePicturePage), typeof(ProfilePicturePage));

            Routing.RegisterRoute(nameof(StudentsReportTutorPage), typeof(StudentsReportTutorPage));
            Routing.RegisterRoute(nameof(SubjectsReportTutorPage), typeof(SubjectsReportTutorPage));
            Routing.RegisterRoute(nameof(SubjectCategoriesReportTutorPage), typeof(SubjectCategoriesReportTutorPage));
            Routing.RegisterRoute(nameof(PlacesReportTutorPage), typeof(PlacesReportTutorPage));

            Routing.RegisterRoute(nameof(StudentsChartTutorPage), typeof(StudentsChartTutorPage));
            Routing.RegisterRoute(nameof(SubjectsChartTutorPage), typeof(SubjectsChartTutorPage));
            Routing.RegisterRoute(nameof(PlacesChartTutorPage), typeof(PlacesChartTutorPage));
            Routing.RegisterRoute(nameof(SubjectCategoriesChartPage), typeof(SubjectCategoriesChartPage));
            Routing.RegisterRoute(nameof(GeneralTimedChartTutorPage), typeof(GeneralTimedChartTutorPage));

            Routing.RegisterRoute(nameof(TutorDetailsStudentPage), typeof(TutorDetailsStudentPage));
            Routing.RegisterRoute(nameof(SearchTutorStudentPage), typeof(SearchTutorStudentPage));

            Routing.RegisterRoute(nameof(ReservationDetailsTutorPage), typeof(ReservationDetailsTutorPage));
            Routing.RegisterRoute(nameof(EditReservationTutorPage), typeof(EditReservationTutorPage));
            Routing.RegisterRoute(nameof(NewReservationTutorPage), typeof(NewReservationTutorPage));
            Routing.RegisterRoute(nameof(NewReservationStudentPage), typeof(NewReservationStudentPage));
            Routing.RegisterRoute(nameof(ReservationDetailsStudentPage), typeof(ReservationDetailsStudentPage));
            Routing.RegisterRoute(nameof(AvailabilitiesTutorPage), typeof(AvailabilitiesTutorPage));
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            SecureStorage.Remove(SecureStorageConstans.Token);
            Settings.LoginStatus = AccountStatus.LoggedOut;
            Shell.Current.GoToAsync($"//{nameof(LoginPage)}").Wait();
        }
    }
}