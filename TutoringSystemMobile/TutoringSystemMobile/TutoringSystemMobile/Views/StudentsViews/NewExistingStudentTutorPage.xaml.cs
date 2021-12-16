using TutoringSystemMobile.ViewModels.StudentViewModels;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewExistingStudentTutorPage
    {
        public NewExistingStudentTutorPage(long studentId)
        {
            InitializeComponent();
            BindingContext = new AddExistingStudentViewModel(studentId);
        }
    }
}