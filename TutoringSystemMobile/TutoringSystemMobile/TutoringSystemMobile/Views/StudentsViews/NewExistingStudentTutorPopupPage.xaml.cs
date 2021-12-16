using TutoringSystemMobile.ViewModels.StudentViewModels;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewExistingStudentTutorPopupPage
    {
        public NewExistingStudentTutorPopupPage(long studentId)
        {
            InitializeComponent();
            BindingContext = new AddExistingStudentViewModel(studentId);
        }
    }
}