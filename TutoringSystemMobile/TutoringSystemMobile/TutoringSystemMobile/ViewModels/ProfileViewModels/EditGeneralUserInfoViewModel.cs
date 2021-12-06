using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using Xamarin.Essentials;
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
        public Command EditUserCommand { get; set; }

        public EditGeneralUserInfoViewModel()
        {
            PageAppearingCommand = new Command(async () => await OnAppearing());
            EditUserCommand = new Command(async () => await OnEditUser(), CanEditUser);
            PropertyChanged += (_, __) => EditUserCommand.ChangeCanExecute();
        }

        public bool CanEditUser()
        {
            return !FirstName.IsEmpty() &&
                !IsBusy;
        }

        private async Task OnEditUser()
        {
            IsBusy = true;
            var updated = await DependencyService.Get<IUserService>().UpdateGeneralUserInfoAsync(new
                UpdatedUserDto(FirstName, LastName));
            IsBusy = false;

            if (updated)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.Updated);
                await SecureStorage.SetAsync(SecureStorageConstans.UserName, $"{FirstName} {LastName}");
                MessagingCenter.Send(this, MessagingCenterConstans.NameChanged);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
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