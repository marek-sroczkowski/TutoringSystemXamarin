using TutoringSystemMobile.ViewModels.Subject;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewSubjectTutorPage : ContentPage
    {
        private readonly NewSubjectViewModel viewModel;

        public NewSubjectTutorPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new NewSubjectViewModel();
        }

        private async void SelectedPlaceEntry_Focused(object sender, FocusEventArgs e)
        {
            await viewModel.OnSelectPlace();
        }
    }
}