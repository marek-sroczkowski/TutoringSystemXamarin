using Firebase.Storage;
using System.IO;
using System.Threading.Tasks;

namespace TutoringSystemMobile.Helpers
{
    public class FirebaseStorageManager
    {
        public static async Task<string> StoreImage(Stream imageStream, string fileName)
        {
            var imageUrl = await new FirebaseStorage(Settings.FirebaseStorageUrl)
                .Child("ProfilePictures")
                .Child(fileName)
                .PutAsync(imageStream);

            return imageUrl;
        }

        public static async Task RemoveImageFirebase(string fileName)
        {
            await new FirebaseStorage(Settings.FirebaseStorageUrl)
                .Child("ProfilePictures")
                .Child(fileName)
                .DeleteAsync();
        }
    }
}