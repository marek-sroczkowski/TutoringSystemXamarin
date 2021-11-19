using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.ProfileViewModels
{
    public class ProfilePictureViewModel : BaseViewModel
    {
        private const float resizedWidth = 512;
        private const float resizedHeight = 512;
        private const string defaultPhoto = "default_user_picture.png";

        private long userId;
        private ImageSource profileImage;
        private bool hasUserPhoto;

        public long UserId { get => userId; set => SetValue(ref userId, value); }
        public ImageSource ProfileImage { get => profileImage; set => SetValue(ref profileImage, value); }
        public bool HasUserPhoto { get => hasUserPhoto; set => SetValue(ref hasUserPhoto, value); }

        public Command PageAppearingCommand { get; }
        public Command RemoveImageCommand { get; }
        public Command SetImageCommand { get; }

        public ProfilePictureViewModel()
        {
            PageAppearingCommand = new Command(OnAppearing);
            RemoveImageCommand = new Command(OnRemoveImage);
            SetImageCommand = new Command(OnSetImage);
        }

        private async void OnSetImage(object obj)
        {
            const string fromGallery = "Galeria";
            const string fromCamera = "Aparat";
            var result = await Shell.Current.DisplayActionSheet("", "Anuluj", null, fromGallery, fromCamera);
            if (result == fromGallery)
            {
                await LoadImageFromGallery();
            }
            else if (result == fromCamera)
            {
                await LoadImageFromCamera();
            }
        }

        private async void OnRemoveImage(object obj)
        {
            var removed = await DependencyService.Get<IImageService>().RemoveProfileImageAsync();
            if (removed)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Usunięto");
                ProfileImage = defaultPhoto;
                HasUserPhoto = false;
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj ponownie później!");
            }
        }

        private async void OnAppearing()
        {
            IsBusy = true;
            var image = await DependencyService.Get<IImageService>()?.GetProfileImageAsync();
            UserId = image.UserId;

            if (string.IsNullOrEmpty(image.ProfilePictureBase64))
            {
                HasUserPhoto = false;
                ProfileImage = defaultPhoto;
            }
            else
            {
                HasUserPhoto = true;
                ProfileImage = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(image.ProfilePictureBase64)));
            }

            IsBusy = false;
        }

        private async Task LoadImageFromGallery()
        {
            try
            {
                await LoadPhotoAsync();
            }
            catch (PermissionException)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Aby załadować zdjęcie musisz wyrazić zgodę na przydzielenie uprawnień aplikacji!");
            }
            catch (Exception)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj ponownie później!");
            }
        }

        private async Task LoadImageFromCamera()
        {
            try
            {
                await TakePhotoAsync();
            }
            catch (PermissionException)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Aby użyć aparatu musisz wyrazić zgodę na przydzielenie uprawnień aplikacji!");
            }
            catch (Exception)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj ponownie później!");
            }
        }

        private async Task TakePhotoAsync()
        {
            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { DefaultCamera = CameraDevice.Front });

            if (photo != null)
            {
                await TrySetPhoto(photo);
            }
        }

        private async Task LoadPhotoAsync()
        {
            var photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());

            if (photo != null)
            {
                await TrySetPhoto(photo);
            }
        }

        private async Task TrySetPhoto(MediaFile photo)
        {
            using (var stream = photo.GetStreamWithImageRotatedForExternalStorage())
            {
                byte[] filebytearray = new byte[stream.Length];
                stream.Read(filebytearray, 0, (int)stream.Length);
                byte[] resizedImage = DependencyService.Get<IImageTransformationService>().ResizeImage(filebytearray, resizedWidth, resizedHeight);
                string base64 = Convert.ToBase64String(resizedImage);
                var set = await DependencyService.Get<IImageService>().SetProfileImageAsync(new ProfileImageDto(base64));
                if (set)
                {
                    ProfileImage = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(base64)));
                    HasUserPhoto = true;
                }
                else
                {
                    DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj ponownie później!");
                    HasUserPhoto = false;
                }
            }
        }
    }
}