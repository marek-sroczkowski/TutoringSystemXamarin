namespace TutoringSystemMobile.Services.Utils
{
    public interface IImageTransformationService
    {
        byte[] ResizeImage(byte[] imageData, float width, float height, float rotation = 0.0f);
    }
}
