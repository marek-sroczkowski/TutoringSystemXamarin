using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
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

        public Command AddStudentCommand { get; }

        public AddExistingStudentViewModel()
        {
            AddStudentCommand = new Command(async () => await OnAddStudent(), CanAddStudent);
            PropertyChanged += (_, __) => AddStudentCommand.ChangeCanExecute();
        }

        private async Task OnAddStudent()
        {
            IsBusy = true;
            var status = await DependencyService.Get<IStudentService>().AddStudentToTutorAsync(new
                NewExistingStudentDto(Username, double.Parse(HourRate), Note));
            IsBusy = false;

            switch (status)
            {
                case AddStudentToTutorStatus.InternalError:
                    DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
                    break;
                case AddStudentToTutorStatus.Added:
                    DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.AddedStudent);
                    await Shell.Current.GoToAsync($"//{nameof(StudentsTutorPage)}");
                    break;
                case AddStudentToTutorStatus.IncorrectUsername:
                    await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.StudentNotExist, GeneralConstans.Ok);
                    break;
                case AddStudentToTutorStatus.StudentWasAlreadyAdded:
                    await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.StudentAlreadyExist, GeneralConstans.Ok);
                    break;
            }
        }

        public bool CanAddStudent()
        {
            return !Username.IsEmpty() &&
                double.TryParse(HourRate, out double hourRate) &&
                hourRate > 0 &&
                !IsBusy;
        }
    }
}
