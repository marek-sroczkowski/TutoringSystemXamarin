using System.Windows.Input;
using TutoringSystemMobile.Commands.StudentCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.StudentViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EditStudentGeneralInformationViewModel : BaseViewModel
    {
        private long id;
        private string hourRate;
        private string note;

        public long Id
        {
            get => id;
            set
            {
                id = value;
                LoadStudentById(id);
            }
        }
        public string HourRate { get => hourRate; set => SetValue(ref hourRate, value); }
        public string Note { get => note; set => SetValue(ref note, value); }

        public ICommand EditStudentCommand { get; }

        private readonly IStudentService studentService;

        public EditStudentGeneralInformationViewModel()
        {
            studentService = DependencyService.Get<IStudentService>();
            EditStudentCommand = new EditStudentCommand(this, studentService);
        }

        private async void LoadStudentById(long studentId)
        {
            IsBusy = true;

            var student = await studentService.GetStudentByIdAsync(studentId);
            HourRate = student.HourlRate.ToString();
            Note = student.Note;

            IsBusy = false;
        }
    }
}