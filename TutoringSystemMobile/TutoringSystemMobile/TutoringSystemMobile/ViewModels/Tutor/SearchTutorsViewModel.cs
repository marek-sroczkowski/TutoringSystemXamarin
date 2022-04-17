using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Models.Dtos.Tutor;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;
using System.Linq;
using TutoringSystemMobile.Helpers;

namespace TutoringSystemMobile.ViewModels.Tutor
{
    public class SearchTutorsViewModel : BaseViewModel
    {
        private string searchedParams;
        private int currentPage;
        private bool hasNext;
        private bool isRefreshing;

        public ObservableCollection<TutorSimpleDto> Tutors { get; }

        public string SearchedParams { get => searchedParams; set => SetValue(ref searchedParams, value); }
        public int CurrentPage { get => currentPage; set => SetValue(ref currentPage, value); }
        public bool HasNext { get => hasNext; set => SetValue(ref hasNext, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetValue(ref isRefreshing, value); }

        public SearchedUserParameters SearchedUserParameters { get; set; }

        public Command SearchTutorsCommand { get; }
        public Command LoadTutorsCommand { get; }
        public Command ItemTresholdReachedCommand { get; }
        public Command<TutorSimpleDto> TutorTappedCommand { get; }

        private readonly ITutorService tutorService = DependencyService.Get<ITutorService>();
        private readonly IStudentRequestService requestService = DependencyService.Get<IStudentRequestService>();

        public SearchTutorsViewModel()
        {
            Tutors = new ObservableCollection<TutorSimpleDto>();
            SearchedUserParameters = new SearchedUserParameters();
            SearchTutorsCommand = new Command(async () => await OnSearchTutors(), CanSearchTutors);
            PropertyChanged += (_, __) => SearchTutorsCommand.ChangeCanExecute();
            LoadTutorsCommand = new Command(async () => await OnLoadTutors());
            ItemTresholdReachedCommand = new Command(async () => await TutorsTresholdReached());
            TutorTappedCommand = new Command<TutorSimpleDto>(async (tutor) => await OnTutorSelected(tutor));
        }

        private bool CanSearchTutors()
        {
            return !SearchedParams.IsEmpty();
        }

        private async Task OnSearchTutors()
        {
            await OnLoadTutors();
        }

        private async Task OnLoadTutors()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            IsRefreshing = true;
            await GetTutorsAsync();
            IsRefreshing = false;
            IsBusy = false;
        }

        private async Task GetTutorsAsync()
        {
            SearchedUserParameters.PageNumber = 1;
            SearchedUserParameters.PageSize = 50;
            SearchedUserParameters.Params = SearchedParams;
            var tutorsCollection = await tutorService.GetTutorsByParamsAsync(SearchedUserParameters);
            CurrentPage = tutorsCollection.Pagination.CurrentPage;
            HasNext = tutorsCollection.Pagination.HasNext;
            Tutors.Clear();
            tutorsCollection.Tutors.ToList().ForEach(tutor => Tutors.Add(tutor));
        }

        private async Task TutorsTresholdReached()
        {
            if (IsBusy || !HasNext)
            {
                return;
            }

            IsBusy = true;
            SearchedUserParameters.PageNumber = ++CurrentPage;
            SearchedUserParameters.PageSize = 50;
            SearchedUserParameters.Params = SearchedParams;
            var tutorsCollection = await tutorService.GetTutorsByParamsAsync(SearchedUserParameters);
            HasNext = tutorsCollection.Pagination.HasNext;
            tutorsCollection.Tutors.ToList().ForEach(tutor => Tutors.Add(tutor));

            IsBusy = false;
        }

        private async Task OnTutorSelected(TutorSimpleDto tutor)
        {
            if (tutor == null)
            {
                return;
            }

            var result = await Shell.Current.DisplayAlert(AlertConstans.Confirmation,
                $"{AlertConstans.ConfirmationTutorAdding} {tutor.Username} {AlertConstans.ToYourTutors}",
                GeneralConstans.Yes, GeneralConstans.No);

            if (result)
            {
                await TryAddTutor(tutor);
            }
        }

        private async Task TryAddTutor(TutorSimpleDto tutor)
        {
            IsBusy = true;
            var status = await requestService.AddRequestAsync(tutor.Id);
            IsBusy = false;

            switch (status)
            {
                case AddTutorToStudentStatus.InternalError:
                case AddTutorToStudentStatus.IncorrectTutor:
                    ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
                    break;
                case AddTutorToStudentStatus.RequestCreated:
                    await Application.Current.MainPage.DisplayAlert(AlertConstans.RequestSent, AlertConstans.NewTutorRequestCreated, GeneralConstans.Ok);
                    await Shell.Current.GoToAsync($"//{nameof(TutorsStudentPage)}");
                    break;
                case AddTutorToStudentStatus.RequestWasAlreadyCreated:
                    await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.RequestWasAlreadyCreated, GeneralConstans.Ok);
                    break;
                case AddTutorToStudentStatus.TutorWasAlreadyAdded:
                    await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.TutorAlreadyExist, GeneralConstans.Ok);
                    break;
            }
        }
    }
}