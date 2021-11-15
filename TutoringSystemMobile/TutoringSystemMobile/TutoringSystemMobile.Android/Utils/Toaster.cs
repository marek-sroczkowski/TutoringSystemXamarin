using Android.Widget;
using TutoringSystemMobile.Droid.Utils;
using TutoringSystemMobile.Services.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(Toaster))]
namespace TutoringSystemMobile.Droid.Utils
{
    public class Toaster : IToast
    {
        public void MakeLongToast(string message)
        {
            Toast.MakeText(Platform.AppContext, message, ToastLength.Long).Show();
        }

        public void MakeShortToast(string message)
        {
            Toast.MakeText(Platform.AppContext, message, ToastLength.Short).Show();
        }
    }
}