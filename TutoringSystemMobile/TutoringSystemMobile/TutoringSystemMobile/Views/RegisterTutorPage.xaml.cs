using TutoringSystemMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterTutorPage : ContentPage
    {
        public RegisterTutorPage()
        {
            InitializeComponent();
            BindingContext = new RegisterTutorViewModel();
        }
    }
}