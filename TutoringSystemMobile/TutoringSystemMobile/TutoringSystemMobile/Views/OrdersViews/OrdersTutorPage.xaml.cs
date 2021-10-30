using TutoringSystemMobile.ViewModels.OrderViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersTutorPage : ContentPage
    {
        private OrdersViewModel viewModel;

        public OrdersTutorPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new OrdersViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}