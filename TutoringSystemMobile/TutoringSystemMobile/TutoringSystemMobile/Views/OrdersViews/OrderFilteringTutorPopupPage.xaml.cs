using TutoringSystemMobile.ViewModels.OrderViewModels;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderFilteringTutorPopupPage
    {
        public OrderFilteringTutorPopupPage(OrdersViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}