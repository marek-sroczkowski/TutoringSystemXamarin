using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.AddressViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.StudentViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class StudentDetailsViewModel : BaseViewModel
    {
        private long id;
        private string name;
        private string username;
        private string hourRate;
        private string note;
        private StudentDetailsDto selectedStudent;

        public long Id
        {
            get => id;
            set
            {
                id = value;
                LoadStudentById(id);
            }
        }

        public string Name { get => name; set => SetValue(ref name, value); }
        public string Username { get => username; set => SetValue(ref username, value); }
        public string HourRate { get => hourRate; set => SetValue(ref hourRate, value); }
        public string Note { get => note; set => SetValue(ref note, value); }

        public Command PageAppearingCommand { get; }
        public Command EditStudentCommand { get; }
        public Command RemoveStudentCommand { get; }

        private readonly IStudentService studentService;

        public StudentDetailsViewModel()
        {
            studentService = DependencyService.Get<IStudentService>();
            PageAppearingCommand = new Command(async () => await OnAppearing());
            EditStudentCommand = new Command(async () => await OnEditStudent());
            RemoveStudentCommand = new Command(async () => await OnRemoveStudent());
        }

        private async void LoadStudentById(long studentId)
        {
            IsBusy = true;

            selectedStudent = await studentService.GetStudentByIdAsync(studentId);
            await SecureStorage.SetAsync(SecureStorageConstans.AddressId, selectedStudent.Address.Id.ToString());
            await SecureStorage.SetAsync(SecureStorageConstans.ContactId, selectedStudent.Contact.Id.ToString());

            Name = $"{selectedStudent.FirstName} {selectedStudent.LastName}";
            Username = selectedStudent.Username;
            HourRate = $"{selectedStudent.HourlRate} zł";
            Note = selectedStudent.Note;

            IsBusy = false;
        }

        private async Task OnAppearing()
        {
            await SecureStorage.SetAsync(SecureStorageConstans.CurrentPage, SecureStorageConstans.GeneralInformation);
        }

        private async Task OnRemoveStudent()
        {
            var result = await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, $"{AlertConstans.ConfirmationRemovePrefix} {Username} {AlertConstans.FromStudentList}", GeneralConstans.Yes, GeneralConstans.No);
            if (result)
                await RemoveStudentAsync();
        }

        private async Task RemoveStudentAsync()
        {
            var removed = await studentService.RemoveStudentAsync(Id);
            if (removed)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.StudentRemoved);
                await DependencyService.Get<IImageSynchronizer>().SynchronizeStudentImages();
                await Shell.Current.GoToAsync($"//{nameof(StudentsTutorPage)}");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async Task OnEditStudent()
        {
            string currentPage = await SecureStorage.GetAsync(SecureStorageConstans.CurrentPage);

            switch(currentPage)
            {
                case SecureStorageConstans.GeneralInformation:
                    await Shell.Current.GoToAsync($"{nameof(EditStudentGeneralInformationTutorPage)}?{nameof(EditStudentGeneralInformationViewModel.Id)}={selectedStudent.Id}");
                    break;
                case SecureStorageConstans.Address:
                    await Shell.Current.GoToAsync($"{nameof(EditAddressPage)}?{nameof(EditAddressViewModel.Id)}={selectedStudent.Address.Id}");
                    break;
                case SecureStorageConstans.Contact:
                    await PopupNavigation.Instance.PushAsync(new EditContactPopupPage(selectedStudent.Contact.Id));
                    break;
            }
        }
    }
}