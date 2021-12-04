using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.StudentViewModels
{
    public class StudentsViewModel : BaseViewModel
    {
        public ObservableCollection<DisplayedStudentDto> Students { get; }

        public Command LoadStudentsCommand { get; }
        public Command NewStudentCommand { get; }
        public Command<StudentDto> StudentTapped { get; }
        public Command PageAppearingCommand { get; }
        public Command RemoveAllStudentsCommand { get; }

        public readonly IStudentService studentService;

        public StudentsViewModel()
        {
            Students = new ObservableCollection<DisplayedStudentDto>();
            studentService = DependencyService.Get<IStudentService>();
            LoadStudentsCommand = new Command(async () => await LoadStudentsAsync());
            NewStudentCommand = new Command(async () => await OnNewStudentClick());
            StudentTapped = new Command<StudentDto>(async (student) => await OnStudentSelected(student));
            PageAppearingCommand = new Command(OnAppearing);
            RemoveAllStudentsCommand = new Command(async () => await OnRemoveAllStudent());
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
            foreach (var student in disyplayedStudents)
                Students.Add(student);

            IsBusy = false;
        }

        private async Task OnStudentSelected(StudentDto student)
        {
            if (student == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(StudentDetailsTutorPage)}?{nameof(StudentDetailsViewModel.Id)}={student.Id}");
        }

        private async Task OnRemoveAllStudent()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Uwaga!", "Czy na pewno chcesz usunąć wszystkich studentów?", "Tak", "Nie");
            if (result)
                await RemoveStudentsAsync();
        }

        private async Task RemoveStudentsAsync()
        {
            var removed = await studentService.RemoveAllStudentsAsync();
            if (removed)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Wyczyszczono listę uczniów!");
                Students.Clear();
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj później!");
            }
        }

        private async Task OnNewStudentClick()
        {
            const string existingStudent = "Student posiadający już konto";
            const string newStudent = "Student nie posiadający konta";
            var result = await Shell.Current.DisplayActionSheet("Nowy student", "Anuluj", null, newStudent, existingStudent);
            if (result == existingStudent)
            {
                await Shell.Current.GoToAsync($"{nameof(NewExistingStudentTutorPage)}");
            }
            else if(result == newStudent)
            {
                await Shell.Current.GoToAsync($"{nameof(CreatingNewStudentTutorPage)}");
            }
        }
    }
}
