using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TutoringSystemMobile.Commands.StudentCommands;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.StudentViewModels
{
    public class StudentsViewModel : BaseViewModel
    {
        private StudentDto selectedStudent;
        public ObservableCollection<StudentDto> Students { get; }

        public StudentDto SelectedStudent
        {
            get => selectedStudent;
            set
            {
                SetValue(ref selectedStudent, value);
                OnStudentSelected(value);
            }
        }

        public ICommand LoadStudentsCommand { get; }
        public Command NewStudentCommand { get; }
        public Command<StudentDto> StudentTapped { get; }
        public Command PageAppearingCommand { get; }
        public Command RemoveAllStudentsCommand { get; }

        public readonly IStudentService studentService;

        public StudentsViewModel()
        {
            Students = new ObservableCollection<StudentDto>();
            studentService = DependencyService.Get<IStudentService>();
            LoadStudentsCommand = new LoadStudentsCommand(this, studentService);
            NewStudentCommand = new Command(OnNewStudentClick);
            StudentTapped = new Command<StudentDto>(OnStudentSelected);
            PageAppearingCommand = new Command(OnAppearing);
            RemoveAllStudentsCommand = new Command(OnRemoveAllStudent);
        }

        private void OnAppearing()
        {
            IsBusy = true;
            selectedStudent = null;
        }

        private async void OnStudentSelected(StudentDto student)
        {
            if (student == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(StudentDetailsTutorPage)}?{nameof(StudentDetailsViewModel.Id)}={student.Id}");
        }

        private async void OnRemoveAllStudent()
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

        private async void OnNewStudentClick()
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
