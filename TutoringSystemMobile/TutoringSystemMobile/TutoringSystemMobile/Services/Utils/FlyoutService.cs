using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using Xamarin.Forms;

[assembly: Dependency(typeof(FlyoutService))]
namespace TutoringSystemMobile.Services.Utils
{
    public class FlyoutService : IFlyoutService
    {
        public void EnableTutorFlyoutItems()
        {
            MessagingCenter.Send(this, Role.Tutor.ToString());
        }

        public void EnableStudentFlyoutItems()
        {
            MessagingCenter.Send(this, Role.Student.ToString());
        }
    }
}
