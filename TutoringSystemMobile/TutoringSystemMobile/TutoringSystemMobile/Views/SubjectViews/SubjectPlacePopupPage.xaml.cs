using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.ViewModels.SubjectViewModels;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubjectPlacePopupPage
    {
        public SubjectPlacePopupPage(SubjectPlace? place)
        {
            InitializeComponent();
            BindingContext = new SubjectPlaceViewModel(place);
        }
    }
}