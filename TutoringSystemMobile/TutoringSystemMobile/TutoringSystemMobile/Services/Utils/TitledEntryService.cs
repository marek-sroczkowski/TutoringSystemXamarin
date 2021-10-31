using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.Services.Utils
{
    public class TitledEntryService : ITitledEntryService
    {
        public void EntryToContentView()
        {
            MessagingCenter.Send(this, "entry");
        }
    }
}
