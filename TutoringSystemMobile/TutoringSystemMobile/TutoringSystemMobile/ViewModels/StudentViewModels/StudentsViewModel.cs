using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
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
        public Command<DisplayedStudentDto> StudentTapped { get; }
        public Command PageAppearingCommand { get; }
        public Command RemoveAllStudentsCommand { get; }

        public readonly IStudentService studentService;

        public StudentsViewModel()
        {
            Students = new ObservableCollection<DisplayedStudentDto>();
            studentService = DependencyService.Get<IStudentService>();
            LoadStudentsCommand = new Command(async () => await LoadStudentsAsync());
            NewStudentCommand = new Command(async () => await OnNewStudent());
            StudentTapped = new Command<DisplayedStudentDto>(async (student) => await OnStudentSelected(student));
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
            {
                Students.Add(student);
            }

            IsBusy = false;
        }

        private async Task OnStudentSelected(DisplayedStudentDto student)
        {
            if (student == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(StudentDetailsTutorPage)}?{nameof(StudentDetailsViewModel.Id)}={student.Id}");
        }

        private async Task OnRemoveAllStudent()
        {
            var result = await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.ConfirmationAllStudentsDeletion, GeneralConstans.Yes, GeneralConstans.No);
            if (result)
                await RemoveStudentsAsync();
        }

        private async Task RemoveStudentsAsync()
        {
            var removed = await studentService.RemoveAllStudentsAsync();
            if (removed)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.CleanedStudents);
                Students.Clear();
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async Task OnNewStudent()
        {
            var result = await Shell.Current.DisplayActionSheet(AlertConstans.NewStudent, GeneralConstans.Cancel, null, AlertConstans.NotExistingStudent, AlertConstans.ExistingStudent);
            if (result == AlertConstans.ExistingStudent)
            {
                await Shell.Current.GoToAsync($"{nameof(NewExistingStudentTutorPage)}");
            }
            else if(result == AlertConstans.NotExistingStudent)
            {
                await Shell.Current.GoToAsync($"{nameof(CreatingNewStudentTutorPage)}");
            }
        }
    }
}