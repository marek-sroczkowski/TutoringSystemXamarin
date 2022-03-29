using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.StudentRequest;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Student
{
    public class StudentRequestsViewModel : BaseViewModel
    {
        public ObservableCollection<StudentRequestDto> Requests { get; }

        public Command LoadRequestsCommand { get; }
        public Command<StudentRequestDto> AcceptRequestCommand { get; }
        public Command<StudentRequestDto> DeclineRequestCommand { get; }
        public Command PageAppearingCommand { get; }

        public readonly IStudentRequestService requestService = DependencyService.Get<IStudentRequestService>();

        public StudentRequestsViewModel()
        {
            Requests = new ObservableCollection<StudentRequestDto>();
            LoadRequestsCommand = new Command(async () => await LoadRequestsAsync());
            AcceptRequestCommand = new Command<StudentRequestDto>(async (request) => await AcceptRequest(request));
            DeclineRequestCommand = new Command<StudentRequestDto>(async (request) => await DeclineRequest(request));
            PageAppearingCommand = new Command(OnAppearing);
        }

        private void OnAppearing()
        {
            IsBusy = true;
        }

        private async Task LoadRequestsAsync()
        {
            IsBusy = true;

            var requests = await requestService.GetRequestsByTutorId();
            Requests.Clear();
            requests.ToList().ForEach(request => Requests.Add(request));

            IsBusy = false;
        }

        private async Task DeclineRequest(StudentRequestDto request)
        {
            if (request is null)
            {
                return;
            }

            var result = await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.ConfirmationStudentRequestDeletion, GeneralConstans.Yes, GeneralConstans.No);
            if (result)
            {
                await RemoveRequestAsync(request.Id);
            }
        }

        private async Task RemoveRequestAsync(long requestId)
        {
            var removed = await requestService.DeclineRequest(requestId);
            if (removed)
            {
                ToastHelper.MakeLongToast(ToastConstans.StudentRequestRemoved);
                await LoadRequestsAsync();
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async Task AcceptRequest(StudentRequestDto request)
        {
            if (request is null)
            {
                return;
            }

            await PopupNavigation.Instance.PushAsync(new NewExistingStudentTutorPopupPage(request.StudentId));
        }
    }
}