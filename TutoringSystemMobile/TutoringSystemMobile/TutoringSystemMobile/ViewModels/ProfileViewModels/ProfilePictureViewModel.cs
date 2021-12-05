using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
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
            PageAppearingCommand = new Command(async () => await OnAppearing());
            RemoveImageCommand = new Command(async () => await OnRemoveImage());
            SetImageCommand = new Command(async () => await OnSetImage());
        }

        private async Task OnSetImage()
        {
            var result = await Shell.Current.DisplayActionSheet(string.Empty, GeneralConstans.Cancel, null, AlertConstans.Galerry, AlertConstans.Camera);
            if (result == AlertConstans.Galerry)
            {
                await LoadImageFromGallery();
            }
            else if (result == AlertConstans.Camera)
            {
                await LoadImageFromCamera();
            }
        }

        private async Task OnRemoveImage()
        {
            var removed = await DependencyService.Get<IImageService>().RemoveProfileImageAsync();
            if (removed)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.Removed);
                ProfileImage = ResourceConstans.DefaultUserPicture;
                HasUserPhoto = false;
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async Task OnAppearing()
        {
            IsBusy = true;
            var image = await DependencyService.Get<IImageService>()?.GetProfileImageAsync();
            UserId = image.UserId;

            if (string.IsNullOrEmpty(image.ProfilePictureBase64))
            {
                HasUserPhoto = false;
                ProfileImage = ResourceConstans.DefaultUserPicture;
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
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.NoGalleryPermission);
            }
            catch (Exception)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
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
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.NoCameraPermission);
            }
            catch (Exception)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
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
                    MessagingCenter.Send(this, MessagingCenterConstans.PhotoChanged);
                }
                else
                {
                    DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
                    HasUserPhoto = false;
                }
            }
        }
    }
}