using System.Threading.Tasks;
using TutoringSystemMobile.Services.Synchronization;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Start
{
    public class StudentStartPageViewModel : BaseViewModel
    {
        public Command PageAppearingCommand { get; }

        public StudentStartPageViewModel()
        {
            PageAppearingCommand = new Command(async () => await OnAppearing());
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            await SynchronizationService.Instance.StartSynchronization();
        }
    }
}