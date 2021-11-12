using System.Windows.Input;
using TutoringSystemMobile.Commands.StudentCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.StudentViewModels
{
    public class AddExistingStudentViewModel : BaseViewModel
    {
        private string username;
        private string hourRate;
        private string note;

        public string Username { get => username; set => SetValue(ref username, value); }
        public string HourRate { get => hourRate; set => SetValue(ref hourRate, value); }
        public string Note { get => note; set => SetValue(ref note, value); }

        public ICommand AddStudentCommand { get; }

        public AddExistingStudentViewModel()
        {
            AddStudentCommand = new AddExistingStudentCommand(this, DependencyService.Get<IStudentService>());
        }
    }
}
