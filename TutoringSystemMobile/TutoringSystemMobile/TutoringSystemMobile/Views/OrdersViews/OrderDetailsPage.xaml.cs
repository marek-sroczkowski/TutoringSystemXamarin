using TutoringSystemMobile.ViewModels.OrderViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailsPage : ContentPage
    {
        public OrderDetailsPage()
        {
            InitializeComponent();
            BindingContext = new OrderDetailsViewModel();
        }
    }
}