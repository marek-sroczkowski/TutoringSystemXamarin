namespace TutoringSystemMobile.Services.Utils
{
    public interface IImageResizer
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
    }
}
