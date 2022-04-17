using System.Drawing;

namespace TutoringSystemMobile.Helpers
{
    public interface IEnvironment
    {
        void SetStatusBarColor(Color color, bool darkStatusBarTint);

        string GetDeviceId();
    }
}