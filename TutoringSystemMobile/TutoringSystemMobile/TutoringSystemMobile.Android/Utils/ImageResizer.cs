using TutoringSystemMobile.Services.Utils;
using Android.Graphics;
using System.IO;
using Xamarin.Forms;
using TutoringSystemMobile.Droid.Utils;

[assembly: Dependency(typeof(ImageResizer))]
namespace TutoringSystemMobile.Droid.Utils
{
    public class ImageResizer : IImageResizer
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

            using (var ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}