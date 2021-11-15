namespace TutoringSystemMobile.Services.Utils
{
    public interface IToast
    {
        void MakeLongToast(string message);
        void MakeShortToast(string message);
    }
}
