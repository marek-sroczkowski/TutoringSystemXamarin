using TutoringSystemMobile.Commands.AccountCommands;
using TutoringSystemMobile.Models.Enums;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {
        private bool isTutor = true;
        private bool isStudent = true;

        public bool IsTutor
        {
            get => isTutor;
            set => SetValue(ref isTutor, value);
        }
        public bool IsStudent
        {
            get => isStudent;
            set => SetValue(ref isStudent, value);
        }

        public AppShellViewModel()
        {
            MessagingCenter.Subscribe<LoginCommand>(this, message: Role.Tutor.ToString(), (sender) =>
            {
                IsTutor = true;
                IsStudent = false;
            });

            MessagingCenter.Subscribe<LoginCommand>(this, message: Role.Student.ToString(), (sender) =>
            {
                IsTutor = false;
                IsStudent = true;
            });

            MessagingCenter.Subscribe<App>(this, message: Role.Tutor.ToString(), (sender) =>
            {
                IsTutor = true;
                IsStudent = false;
            });

            MessagingCenter.Subscribe<App>(this, message: Role.Student.ToString(), (sender) =>
            {
                IsTutor = false;
                IsStudent = true;
            });
        }
    }
}
