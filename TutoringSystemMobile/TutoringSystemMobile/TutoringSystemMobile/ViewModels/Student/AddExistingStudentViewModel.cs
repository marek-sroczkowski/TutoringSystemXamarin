using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Dtos.Student;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Student
{
    public class AddExistingStudentViewModel : BaseViewModel
    {
        private long studentId;
        private string hourRate;
        private string note;

        public long StudentId { get => studentId; set => SetValue(ref studentId, value); }
        public string HourRate { get => hourRate; set => SetValue(ref hourRate, value); }
        public string Note { get => note; set => SetValue(ref note, value); }

        public Command AddStudentCommand { get; }

        public AddExistingStudentViewModel(long studentId)
        {
            StudentId = studentId;
            AddStudentCommand = new Command(async () => await OnAddStudent(), CanAddStudent);
            PropertyChanged += (_, __) => AddStudentCommand.ChangeCanExecute();
        }

        private async Task OnAddStudent()
        {
            IsBusy = true;
            var status = await DependencyService.Get<IStudentService>().AddStudentToTutorAsync(new
                NewExistingStudentDto(StudentId, double.Parse(HourRate), Note));
            IsBusy = false;

            switch (status)
            {
                case AddStudentToTutorStatus.InternalError:
                    DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
                    break;
                case AddStudentToTutorStatus.Added:
                    DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.AddedStudent);
                    await DependencyService.Get<IImageSynchronizer>().SynchronizeStudentImages();
                    await Shell.Current.GoToAsync($"//{nameof(StudentsTutorPage)}");
                    break;
                case AddStudentToTutorStatus.IncorrectUsername:
                    await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.StudentNotExist, GeneralConstans.Ok);
                    break;
                case AddStudentToTutorStatus.StudentWasAlreadyAdded:
                    await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.StudentAlreadyExist, GeneralConstans.Ok);
                    break;
            }

            await PopupNavigation.Instance.PopAsync();
        }

        public bool CanAddStudent()
        {
            return double.TryParse(HourRate, out double hourRate) &&
                hourRate > 0 &&
                !IsBusy;
        }
    }
}