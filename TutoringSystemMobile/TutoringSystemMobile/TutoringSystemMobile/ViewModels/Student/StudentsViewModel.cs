using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Dtos.Student;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Student
{
    public class StudentsViewModel : BaseViewModel
    {
        public ObservableCollection<DisplayedStudentDto> Students { get; }

        public Command LoadStudentsCommand { get; }
        public Command NewStudentCommand { get; }
        public Command<DisplayedStudentDto> StudentTapped { get; }
        public Command PageAppearingCommand { get; }
        public Command StudentRequestsCommand { get; }

        public readonly IStudentService studentService = DependencyService.Get<IStudentService>();

        public StudentsViewModel()
        {
            Students = new ObservableCollection<DisplayedStudentDto>();
            LoadStudentsCommand = new Command(async () => await LoadStudentsAsync());
            NewStudentCommand = new Command(async () => await OnNewStudent());
            StudentTapped = new Command<DisplayedStudentDto>(async (student) => await OnStudentSelected(student));
            PageAppearingCommand = new Command(OnAppearing);
            StudentRequestsCommand = new Command(async () => await OnStudentRequests());
        }

        private void OnAppearing()
        {
            IsBusy = true;
        }

        private async Task LoadStudentsAsync()
        {
            IsBusy = true;

            var students = await studentService.GetStudentsAsync();
            var disyplayedStudents = students.Select(s => new DisplayedStudentDto(s));
            Students.Clear();
            disyplayedStudents.ToList().ForEach(student => Students.Add(student));

            IsBusy = false;
        }

        private async Task OnStudentSelected(DisplayedStudentDto student)
        {
            if (student is null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(StudentDetailsTutorPage)}?{nameof(StudentDetailsViewModel.Id)}={student.Id}");
        }

        private async Task OnNewStudent()
        {
            var result = await Shell.Current.DisplayActionSheet(AlertConstans.NewStudent, GeneralConstans.Cancel, null, AlertConstans.NotExistingStudent, AlertConstans.ExistingStudent);
            if (result == AlertConstans.ExistingStudent)
            {
                await Shell.Current.GoToAsync($"{nameof(SearchStudentPage)}");
            }
            else if (result == AlertConstans.NotExistingStudent)
            {
                await Shell.Current.GoToAsync($"{nameof(CreatingNewStudentTutorPage)}");
            }
        }

        private async Task OnStudentRequests()
        {
            await Shell.Current.GoToAsync($"{nameof(StudentRequestsTutorPage)}");
        }
    }
}