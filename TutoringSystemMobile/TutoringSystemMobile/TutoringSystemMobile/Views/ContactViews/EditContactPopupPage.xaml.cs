using TutoringSystemMobile.ViewModels.ContactViewModels;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditContactPopupPage
    {
        public EditContactPopupPage(long contactId)
        {
            InitializeComponent();
            BindingContext = new EditContactViewModel(contactId);
        }
    }
}