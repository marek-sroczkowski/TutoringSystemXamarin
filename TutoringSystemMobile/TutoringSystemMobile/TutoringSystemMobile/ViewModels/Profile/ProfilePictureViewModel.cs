using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Images;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Profile
{
    public class ProfilePictureViewModel : BaseViewModel
    {
        private long userId;
        private bool hasUserPhoto;
        private ImageSource profileImage;

        public long UserId { get => userId; set => SetValue(ref userId, value); }
        public bool HasUserPhoto { get => hasUserPhoto; set => SetValue(ref hasUserPhoto, value); }
        public ImageSource ProfileImage { get => profileImage; set => SetValue(ref profileImage, value); }

        public Command PageAppearingCommand { get; }
        public Command RemoveImageCommand { get; }
        public Command SetImageCommand { get; }

        private readonly IImageService imageService = DependencyService.Get<IImageService>();

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
            {
                return;
            }

            IsBusy = true;
            var removed = await imageService.RemoveProfileImageAsync();

            if (removed)
            {
                await FirebaseStorageManager.RemoveImageFirebase($"{userId}.jpg");
                ToastHelper.MakeLongToast(ToastConstans.Removed);
                ProfileImage = ResourceConstans.DefaultUserPicture;
                MessagingCenter.Send(this, MessagingCenterConstans.PhotoChanged);
                HasUserPhoto = false;
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }

            IsBusy = false;
        }

        private async Task OnAppearing()
        {
            var image = await imageService.GetProfileImageAsync();
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
                ToastHelper.MakeLongToast(ToastConstans.NoGalleryPermission);
            }
            catch (Exception)
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
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
                ToastHelper.MakeLongToast(ToastConstans.NoCameraPermission);
            }
            catch (Exception)
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
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
                var set = await imageService.SetProfileImageAsync(new ProfileImageDto(imageUrl));

                if (set)
                {
                    ProfileImage = ImageSource.FromUri(new Uri(imageUrl));
                    HasUserPhoto = true;
                    MessagingCenter.Send(this, MessagingCenterConstans.PhotoChanged);
                }
                else
                {
                    await FirebaseStorageManager.RemoveImageFirebase($"{userId}.jpg");
                    ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
                    HasUserPhoto = false;
                }
            }
        }
    }
}