using TutoringSystemMobile.Droid.Utils;
using TutoringSystemMobile.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(OperatingSystemService))]
namespace TutoringSystemMobile.Droid.Utils
{
    public class OperatingSystemService : IOperatingSystemService
    {
        public int GetApiLevel()
        {
            var apiLevel = Android.OS.Build.VERSION.Sdk;

            return int.Parse(apiLevel);
        }
    }
}