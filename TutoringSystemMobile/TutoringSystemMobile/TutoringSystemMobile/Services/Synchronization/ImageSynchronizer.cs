using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.ImagesDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.SQLite;
using TutoringSystemMobile.Services.Synchronization;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageSynchronizer))]
namespace TutoringSystemMobile.Services.Synchronization
{
    public class ImageSynchronizer : IImageSynchronizer
    {
        private readonly WebClient client;
        private IEnumerable<UserImageDto> images;
        private IEnumerable<UserImageDto> localImages;

        public ImageSynchronizer()
        {
            client = new WebClient();
        }

        public async Task SynchronizeStudentImages()
        {
            images = (await DependencyService.Get<IImageService>().GetStudentPhotos())
                .Select(image => new UserImageDto(image, Role.Student));

            localImages = SQLiteManager.Instance.GetAll<UserImageDto>()
                .Where(image => image.UserRole.Equals(Role.Student));

            RemoveDeletingImages(Role.Student);
            await AddNewImages();
            await UpdateExistingImages();
        }

        public async Task SynchronizeTutorImages()
        {
            images = (await DependencyService.Get<IImageService>().GetTutorPhotos())
                .Select(image => new UserImageDto(image, Role.Tutor));

            localImages = SQLiteManager.Instance.GetAll<UserImageDto>()
                .Where(image => image.UserRole.Equals(Role.Tutor));

            RemoveDeletingImages(Role.Tutor);
            await AddNewImages();
            await UpdateExistingImages();
        }

        private async Task UpdateExistingImages()
        {
            var imageUrls = images.Where(image => !string.IsNullOrEmpty(image.ProfilePictureFirebaseUrl))
                .Select(image => image.ProfilePictureFirebaseUrl);
            var toUpdated = localImages.Where(image => !imageUrls.Contains(image.ProfilePictureFirebaseUrl)).ToList();
            for (int i = 0; i < toUpdated.Count; i++)
            {
                toUpdated[i].ProfilePictureFirebaseUrl = images
                    .FirstOrDefault(image => image.UserId.Equals(toUpdated[i].UserId))
                    .ProfilePictureFirebaseUrl;
                byte[] byteImage = await client.DownloadDataTaskAsync(toUpdated[i].ProfilePictureFirebaseUrl);
                toUpdated[i].ProfilePictureBase64 = Convert.ToBase64String(byteImage);
            }

            toUpdated.ForEach(image => SQLiteManager.Instance.Update(image));
        }

        private async Task AddNewImages()
        {
            var userIds = localImages.Select(image => image.UserId);
            var toAdded = images.Where(image => !string.IsNullOrEmpty(image.ProfilePictureFirebaseUrl) && !userIds.Contains(image.UserId)).ToList();
            for (int i = 0; i < toAdded.Count; i++)
            {
                byte[] byteImage = await client.DownloadDataTaskAsync(toAdded[i].ProfilePictureFirebaseUrl);
                toAdded[i].ProfilePictureBase64 = Convert.ToBase64String(byteImage);
            }

            toAdded.ForEach(image => SQLiteManager.Instance.Add(image));
        }

        private void RemoveDeletingImages(Role role)
        {
            var userIds = images.Where(image => string.IsNullOrEmpty(image.ProfilePictureFirebaseUrl))
                .Select(image => image.UserId);
            var toRemoved = localImages.Where(image => userIds.Contains(image.UserId)).ToList();

            toRemoved.ForEach(image => SQLiteManager.Instance.Remove<UserImageDto>(image.UserId));
            localImages = SQLiteManager.Instance.GetAll<UserImageDto>()
                .Where(image => image.UserRole.Equals(role));
        }
    }
}
