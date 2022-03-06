using TutoringSystemMobile.ViewModels.Subject;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditSubjectTutorPage : ContentPage
    {
        private readonly EditSubjectViewModel viewModel;

        public EditSubjectTutorPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new EditSubjectViewModel();
        }

        private async void SelectedPlaceEntry_Focused(object sender, FocusEventArgs e)
        {
            await viewModel.OnSelectPlace();
        }
    }
}