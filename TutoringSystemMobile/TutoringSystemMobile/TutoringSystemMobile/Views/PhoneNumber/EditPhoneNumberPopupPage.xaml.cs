using TutoringSystemMobile.ViewModels.PhoneNumber;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPhoneNumberPopupPage
    {
        public EditPhoneNumberPopupPage(long phoneNumberId)
        {
            InitializeComponent();
            BindingContext = new EditPhoneNumberViewModel(phoneNumberId);
        }
    }
}