using TutoringSystemMobile.Services.Interfaces;
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

        public Command EditStudentCommand { get; }

        private readonly IStudentService studentService;

        public StudentDetailsViewModel()
        {
            studentService = DependencyService.Get<IStudentService>();
        }

        private async void LoadStudentById(long studentId)
        {
            IsBusy = true;

            var student = await studentService.GetStudentByIdAsync(studentId);
            await SecureStorage.SetAsync("addressId", student.Address.Id.ToString());
            await SecureStorage.SetAsync("contactId", student.Contact.Id.ToString());

            Name = $"{student.FirstName} {student.LastName}";
            Username = student.Username;
            HourRate = $"{student.HourlRate} zł";
            Note = student.Note;

            IsBusy = false;
        }
    }
}
