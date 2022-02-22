using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Tutor
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class TutorDetailsViewModel : BaseViewModel
    {
        private long id;
        private string name;
        private string username;
        private string hourRate;

        public long Id
        {
            get => id;
            set
            {
                id = value;
                LoadTutorById(id);
            }
        }

        public string Name { get => name; set => SetValue(ref name, value); }
        public string Username { get => username; set => SetValue(ref username, value); }
        public string HourRate { get => hourRate; set => SetValue(ref hourRate, value); }

        public Command PageAppearingCommand { get; }

        private readonly ITutorService tutorService;

        public TutorDetailsViewModel()
        {
            tutorService = DependencyService.Get<ITutorService>();
            PageAppearingCommand = new Command(async () => await OnAppearing());
        }

        private async void LoadTutorById(long tutorId)
        {
            IsBusy = true;

            var tutor = await tutorService.GetTutorByIdAsync(tutorId);
            await SecureStorage.SetAsync(SecureStorageConstans.AddressId, tutor.Address.Id.ToString());
            await SecureStorage.SetAsync(SecureStorageConstans.ContactId, tutor.Contact.Id.ToString());

            Name = $"{tutor.FirstName} {tutor.LastName}";
            Username = tutor.Username;
            HourRate = $"{tutor.HourlRate} zł";

            IsBusy = false;
        }

        private async Task OnAppearing()
        {
            await SecureStorage.SetAsync(SecureStorageConstans.CurrentPage, SecureStorageConstans.GeneralInformation);
        }
    }
}