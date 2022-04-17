using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Account;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Profile
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

        private readonly IUserService userService = DependencyService.Get<IUserService>();

        public EditGeneralUserInfoViewModel()
        {
            PageAppearingCommand = new Command(async () => await OnAppearing());
            EditUserCommand = new Command(async () => await OnEditUser(), CanEditUser);
            PropertyChanged += (_, __) => EditUserCommand.ChangeCanExecute();
        }

        public bool CanEditUser()
        {
            return !FirstName.IsEmpty()
                && !IsBusy;
        }

        private async Task OnEditUser()
        {
            IsBusy = true;
            var user = new UpdatedUserDto(FirstName, LastName);
            var updated = await userService.UpdateGeneralUserInfoAsync(user);
            IsBusy = false;

            if (updated)
            {
                ToastHelper.MakeLongToast(ToastConstans.Updated);
                await SecureStorage.SetAsync(SecureStorageConstans.UserName, $"{FirstName} {LastName}");
                MessagingCenter.Send(this, MessagingCenterConstans.NameChanged);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async Task OnAppearing()
        {
            var user = await userService.GetGeneralUserInfoAsync();
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
    }
}