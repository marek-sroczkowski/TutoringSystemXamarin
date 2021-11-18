using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.AddressViewModels;
using TutoringSystemMobile.ViewModels.ContactViewModels;
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
            PageAppearingCommand = new Command(OnAppearing);
            EditStudentCommand = new Command(OnEditStudent);
            RemoveStudentCommand = new Command(OnRemoveStudent);
        }

        private async void LoadStudentById(long studentId)
        {
            IsBusy = true;

            selectedStudent = await studentService.GetStudentByIdAsync(studentId);
            await SecureStorage.SetAsync("addressId", selectedStudent.Address.Id.ToString());
            await SecureStorage.SetAsync("contactId", selectedStudent.Contact.Id.ToString());

            Name = $"{selectedStudent.FirstName} {selectedStudent.LastName}";
            Username = selectedStudent.Username;
            HourRate = $"{selectedStudent.HourlRate} zł";
            Note = selectedStudent.Note;

            IsBusy = false;
        }

        private async void OnAppearing()
        {
            await SecureStorage.SetAsync("currentPage", "generalInformation");
        }

        private async void OnRemoveStudent(object obj)
        {
            var result = await Application.Current.MainPage.DisplayAlert("Uwaga!", $"Czy na pewno chcesz usunąć  {Username} z listy swoich uczniów?", "Tak", "Nie");
            if (result)
                await RemoveStudentAsync();
        }

        private async Task RemoveStudentAsync()
        {
            var removed = await studentService.RemoveStudentAsync(Id);
            if (removed)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Skreślono ucznia z listy!");
                await Shell.Current.GoToAsync($"//{nameof(StudentsTutorPage)}");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj później!");
            }
        }

        private async void OnEditStudent()
        {
            string currentPage = await SecureStorage.GetAsync("currentPage");

            switch(currentPage)
            {
                case "generalInformation":
                    await Shell.Current.GoToAsync($"{nameof(EditStudentGeneralInformationTutorPage)}?{nameof(EditStudentGeneralInformationViewModel.Id)}={selectedStudent.Id}");
                    break;
                case "address":
                    await Shell.Current.GoToAsync($"{nameof(EditAddressPage)}?{nameof(EditAddressViewModel.Id)}={selectedStudent.Address.Id}");
                    break;
                case "contact":
                    await PopupNavigation.Instance.PushAsync(new EditContactPopupPage(selectedStudent.Contact.Id));
                    break;
            }
        }
    }
}