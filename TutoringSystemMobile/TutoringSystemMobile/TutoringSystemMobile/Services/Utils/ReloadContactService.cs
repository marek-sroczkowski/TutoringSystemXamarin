using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using Xamarin.Forms;

[assembly: Dependency(typeof(ReloadContactService))]
namespace TutoringSystemMobile.Services.Utils
{
    public class ReloadContactService : IReloadContactService
    {
        public void ReloadContact()
        {
            MessagingCenter.Send(this, "reload");
        }
    }
}
