using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using Xamarin.Forms;

[assembly: Dependency(typeof(FlyoutItemService))]
namespace TutoringSystemMobile.Services.Utils
{
    public class FlyoutItemService : IFlyoutItemService
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
