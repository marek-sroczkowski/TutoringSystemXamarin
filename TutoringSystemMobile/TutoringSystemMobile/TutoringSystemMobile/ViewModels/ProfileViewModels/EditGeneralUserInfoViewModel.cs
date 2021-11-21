using System.Threading.Tasks;
using System.Windows.Input;
using TutoringSystemMobile.Commands.ProfileCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.ProfileViewModels
{
    public class EditGeneralUserInfoViewModel : BaseViewModel
    {
        private long id;
        private string firstName;
        private string lastName;

        public long Id { get => id; set => SetValue(ref id, value); }
        public string FirstName { get => firstName; set => SetValue(ref firstName, value); }
        public string LastName { get => lastName; set => SetValue(ref lastName, value); }

        public Command PageAppearingCommand { get; }
        public ICommand EditUserCommand { get; set; }

        public EditGeneralUserInfoViewModel()
        {
            PageAppearingCommand = new Command(async () => await OnAppearing());
            EditUserCommand = new EditGeneralUserInfoCommand(this, DependencyService.Get<IUserService>());
        }

        private async Task OnAppearing()
        {
            var user = await DependencyService.Get<IUserService>()?.GetGeneralUserInfoAsync();
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
    }
}