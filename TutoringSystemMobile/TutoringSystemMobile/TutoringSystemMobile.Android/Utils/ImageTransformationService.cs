using TutoringSystemMobile.Services.Utils;
using Android.Graphics;
using System.IO;
using Xamarin.Forms;
using TutoringSystemMobile.Droid.Utils;

[assembly: Dependency(typeof(ImageTransformationService))]
namespace TutoringSystemMobile.Droid.Utils
{
    public class ImageTransformationService : IImageTransformationService
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height, float rotation = 0.0f)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            var matrix = new Matrix();
            var scaleWidth = width / originalImage.Width;
            var scaleHeight = height / originalImage.Height;
            matrix.PostRotate(rotation);
            matrix.PreScale(scaleWidth, scaleHeight);
            Bitmap resizedImage = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true);

            using (var ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}