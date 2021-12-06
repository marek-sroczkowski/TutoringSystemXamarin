using System;
using System.IO;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.ProfileViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {
        private bool isTutor = true;
        private bool isStudent = true;
        private string user;
        private ImageSource profileImage = ResourceConstans.DefaultUserPicture;

        public bool IsTutor { get => isTutor; set => SetValue(ref isTutor, value); }
        public bool IsStudent { get => isStudent; set => SetValue(ref isStudent, value); }

        public string User { get => user; set => SetValue(ref user, value); }
        public ImageSource ProfileImage { get => profileImage; set => SetValue(ref profileImage, value); }

        public AppShellViewModel()
        {
            MessagingCenter.Subscribe<FlyoutItemService>(this, message: Role.Tutor.ToString(), async (sender) =>
            {
                IsTutor = true;
                IsStudent = false;
                await LoadUserAsync();
                await LoadPictureAsync();
            });

            MessagingCenter.Subscribe<FlyoutItemService>(this, message: Role.Student.ToString(), async (sender) =>
            {
                IsTutor = false;
                IsStudent = true;
                await LoadUserAsync();
                await LoadPictureAsync();
            });

            MessagingCenter.Subscribe<ProfilePictureViewModel>(this, message: MessagingCenterConstans.PhotoChanged, async (sender) =>
            {
                await LoadPictureAsync();
            });

            MessagingCenter.Subscribe<EditGeneralUserInfoViewModel>(this, message: MessagingCenterConstans.NameChanged, async (sender) =>
            {
                await LoadUserAsync();
            });

            OnAppearing();
        }

        private async void OnAppearing()
        {
            await LoadUserAsync();
            await LoadPictureAsync();
        }

        private async Task LoadUserAsync()
        {
            User = await SecureStorage.GetAsync(SecureStorageConstans.UserName);
            var user = await DependencyService.Get<IUserService>().GetGeneralUserInfoAsync();
            User = $"{user.FirstName} {user.LastName}";
        }

        private async Task LoadPictureAsync()
        {
            var picture = await DependencyService.Get<IImageService>()?.GetProfileImageAsync();
            ProfileImage = picture.ProfilePictureBase64 != null ?
                ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(picture.ProfilePictureBase64))) :
                ResourceConstans.DefaultUserPicture;
        }
    }
}
