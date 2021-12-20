using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.Services.Synchronization
{
    public class SynchronizationService
    {
        private static SynchronizationService instance = null;
        private static readonly object locker = new object();

        public static SynchronizationService Instance
        {
            get
            {
                if (instance is null)
                {
                    lock (locker)
                    {
                        if (instance is null)
                        {
                            instance = new SynchronizationService();
                        }
                    }
                }

                return instance;
            }
        }

        public async Task StartSynchronization()
        {
            var lastSynchronizationDateString = await SecureStorage.GetAsync(SecureStorageConstans.LastSynchronizationDate);
            var lastSynchronizationDate = !string.IsNullOrEmpty(lastSynchronizationDateString) ?
                Convert.ToDateTime(lastSynchronizationDateString) :
                DateTime.Now.AddHours(-10);
            if (lastSynchronizationDate.AddHours(1) > DateTime.Now)
                return;

            await DependencyService.Get<IImageSynchronizer>().SynchronizeStudentImages();

            await SecureStorage.SetAsync(SecureStorageConstans.LastSynchronizationDate, DateTime.Now.ToString());
        }
    }
}