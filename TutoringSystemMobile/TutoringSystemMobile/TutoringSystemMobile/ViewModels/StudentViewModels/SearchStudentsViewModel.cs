using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.StudentViewModels
{
    public class SearchStudentsViewModel : BaseViewModel
    {
        private string searchedParams;
        private int currentPage;
        private bool hasNext;
        private bool isRefreshing;

        public ObservableCollection<StudentSimpleDto> Students { get; }

        public string SearchedParams { get => searchedParams; set => SetValue(ref searchedParams, value); }
        public int CurrentPage { get => currentPage; set => SetValue(ref currentPage, value); }
        public bool HasNext { get => hasNext; set => SetValue(ref hasNext, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetValue(ref isRefreshing, value); }

        public SearchedUserParameters SearchedUserParameters { get; set; }

        public Command SearchStudentsCommand { get; }
        public Command LoadStudentsCommand { get; }
        public Command ItemTresholdReachedCommand { get; }
        public Command<StudentSimpleDto> StudentTappedCommand { get; }

        private readonly IStudentService studentService;

        public SearchStudentsViewModel()
        {
            studentService = DependencyService.Get<IStudentService>();
            Students = new ObservableCollection<StudentSimpleDto>();
            SearchedUserParameters = new SearchedUserParameters();
            SearchStudentsCommand = new Command(async () => await OnSearchStudents(), CanSearchStudents);
            PropertyChanged += (_, __) => SearchStudentsCommand.ChangeCanExecute();
            LoadStudentsCommand = new Command(async () => await OnLoadStudents());
            ItemTresholdReachedCommand = new Command(async () => await StudentsTresholdReached());
            StudentTappedCommand = new Command<StudentSimpleDto>(async (student) => await OnStudentSelected(student));
        }

        private bool CanSearchStudents()
        {
            return !SearchedParams.IsEmpty();
        }

        private async Task OnSearchStudents()
        {
            await OnLoadStudents();
        }

        private async Task OnLoadStudents()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsRefreshing = true;

            SearchedUserParameters.PageNumber = 1;
            SearchedUserParameters.PageSize = 50;
            SearchedUserParameters.Params = SearchedParams;
            var studentsCollection = await studentService.GetTutorsByParamsAsync(SearchedUserParameters);
            CurrentPage = studentsCollection.Pagination.CurrentPage;
            HasNext = studentsCollection.Pagination.HasNext;
            Students.Clear();
            studentsCollection.Students.ToList().ForEach(student => Students.Add(student));

            IsRefreshing = false;
            IsBusy = false;
        }

        private async Task StudentsTresholdReached()
        {
            if (IsBusy || !HasNext)
                return;

            IsBusy = true;

            SearchedUserParameters.PageNumber = ++CurrentPage;
            SearchedUserParameters.PageSize = 50;
            SearchedUserParameters.Params = SearchedParams;
            var studentsCollection = await studentService.GetTutorsByParamsAsync(SearchedUserParameters);
            HasNext = studentsCollection.Pagination.HasNext;
            studentsCollection.Students.ToList().ForEach(student => Students.Add(student));

            IsBusy = false;
        }

        private async Task OnStudentSelected(StudentSimpleDto student)
        {
            if (student is null)
                return;

            await PopupNavigation.Instance.PushAsync(new NewExistingStudentTutorPopupPage(student.Id));
        }
    }
}
