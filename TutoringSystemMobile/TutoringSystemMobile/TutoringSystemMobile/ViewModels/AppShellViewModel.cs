using System;
using System.IO;
using System.Threading.Tasks;
using TutoringSystemMobile.Commands.ProfileCommands;
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
        private ImageSource profileImage = "default_user_picture.png";

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

            MessagingCenter.Subscribe<ProfilePictureViewModel>(this, message: "photoChanged", async (sender) =>
            {
                await LoadPictureAsync();
            });

            MessagingCenter.Subscribe<EditGeneralUserInfoCommand>(this, message: "nameChanged", async (sender) =>
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
            User = await SecureStorage.GetAsync("userName");
        }

        private async Task LoadPictureAsync()
        {
            var picture = await DependencyService.Get<IImageService>()?.GetProfileImageAsync();
            if (picture.ProfilePictureBase64 != null)
                ProfileImage = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(picture.ProfilePictureBase64)));
            else
                ProfileImage = "default_user_picture.png";
        }
    }
}
