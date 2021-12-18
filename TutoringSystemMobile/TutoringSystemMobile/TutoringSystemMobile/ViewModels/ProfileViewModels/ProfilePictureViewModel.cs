using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
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
            HasUserPhoto = false;
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
            if (!HasUserPhoto)
                return;

            IsBusy = true;
            var removed = await DependencyService.Get<IImageService>().RemoveProfileImageAsync();

            if (removed)
            {
                await FirebaseStorageManager.RemoveImageFirebase($"{userId}.jpg");
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.Removed);
                ProfileImage = ResourceConstans.DefaultUserPicture;
                HasUserPhoto = false;
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }

            IsBusy = false;
        }

        private async Task OnAppearing()
        {
            var image = await DependencyService.Get<IImageService>()?.GetProfileImageAsync();
            UserId = image.UserId;

            if (string.IsNullOrEmpty(image.ProfilePictureFirebaseUrl))
            {
                HasUserPhoto = false;
                ProfileImage = ResourceConstans.DefaultUserPicture;
            }
            else
            {
                HasUserPhoto = true;
                ProfileImage = ImageSource.FromUri(new Uri(image.ProfilePictureFirebaseUrl));
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
            finally
            {
                IsBusy = false;
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
            finally
            {
                IsBusy = false;
            }
        }

        private async Task TakePhotoAsync()
        {
            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { DefaultCamera = CameraDevice.Front, PhotoSize = PhotoSize.Small });
            IsBusy = true;

            if (photo != null)
            {
                await TrySetPhoto(photo);
            }
        }

        private async Task LoadPhotoAsync()
        {
            var photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { PhotoSize = PhotoSize.Small });
            IsBusy = true;

            if (photo != null)
            {
                await TrySetPhoto(photo);
            }
        }

        private async Task TrySetPhoto(MediaFile photo)
        {
            using var stream = photo.GetStreamWithImageRotatedForExternalStorage();
            var imageUrl = await FirebaseStorageManager.StoreImage(stream, $"{userId}.jpg");
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var set = await DependencyService.Get<IImageService>().SetProfileImageAsync(new ProfileImageDto(imageUrl));

                if (set)
                {
                    ProfileImage = ImageSource.FromUri(new Uri(imageUrl));
                    HasUserPhoto = true;
                    MessagingCenter.Send(this, MessagingCenterConstans.PhotoChanged);
                }
                else
                {
                    await FirebaseStorageManager.RemoveImageFirebase($"{userId}.jpg");
                    DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
                    HasUserPhoto = false;
                }
            }
        }
    }
}